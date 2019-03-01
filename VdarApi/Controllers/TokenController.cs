using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VdarApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VdarApi.Repositories;
using Newtonsoft.Json;
using VdarApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace VdarApi.Controllers
{
    public class TokenController : Controller
    {
        private IRTokenRepository _tokenRP;

        public TokenController(IRTokenRepository tokenRepository)
        {
            this._tokenRP = tokenRepository;
        }

        private async Task SendResponse(ResponseData response)
        {
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [HttpPost("/token")]
        public async Task Token([FromQuery]Parameters parameters)
        {
            if (parameters == null)
                await SendResponse(new ResponseData(901));

            switch (parameters.grant_type)
            {
                case "password":
                    await SendResponse(DoAuthentication(parameters)); break;
                case "refresh_token":
                    if (User.Identity.IsAuthenticated)
                        await SendResponse(DoRefreshToken(parameters));
                    else
                        await SendResponse(new ResponseData(903));
                    break;
                default:
                    await SendResponse(new ResponseData(902)); break;
            };           

        }

        private ResponseData DoAuthentication(Parameters parameters)
        {
            var _user = UserInfo.GetAllUsers()
                        .SingleOrDefault( z => z.Password.Equals(parameters.password) 
                                            && z.UserName.Equals(parameters.username));
            
            if (_user == null)
                return new ResponseData(904);

            // Удаляем токен пользователя (в разрезе браузера), на случай если он украден
            _tokenRP.RemoveToken(_user.ClientId, parameters.finger_print??"");

            //Формируем новый токен
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var token = new Tokens
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = _user.ClientId,
                AccessToken = GetJwt(_user, _user.GetHashCode().ToString()),
                RefreshToken = refresh_token,
                UpdateHashSum = _user.GetHashCode().ToString(),
                FingerPrint = parameters.finger_print??"",
                CreatedDateUTC = DateTime.UtcNow
            };

            //Добавляем токен в таблицу БД
            if (_tokenRP.AddToken(token))
                return new ResponseData(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
            else
                return new ResponseData(905);
        }

        private ResponseData DoRefreshToken(Parameters parameters)
        {
            string access_token = ControllerContext.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var token = _tokenRP.GetToken(parameters.finger_print??"", access_token, parameters.refresh_token);
            
            if (token == null)
                return new ResponseData(906);

            UserInfo _user = new UserInfo()
            {
                ClientId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault()
            };

            string claims_hash = User.FindFirstValue(ClaimTypes.Hash);


            if (token.UpdateHashSum.Equals(claims_hash))
            {
                //подтягиваем данные из существующего JWT
                _user.UserName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
                _user.UserRole = User.FindFirstValue(ClaimsIdentity.DefaultRoleClaimType);
            }
            else
            {
                //подтягиваем данные из БД
                _user = UserInfo.GetAllUsers()
                           .SingleOrDefault(z => z.ClientId.Equals(_user.ClientId));

                if (_user == null)
                    return new ResponseData(904);

                claims_hash = token.UpdateHashSum;
            }

            token.CreatedDateUTC = DateTime.UtcNow;
            token.AccessToken = GetJwt(_user, claims_hash);
            if (_tokenRP.RefreshToken(token))
                return new ResponseData(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
            else
                return new ResponseData(907);
        }


        private string GetJwt(UserInfo info, string clientHash)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, info.ClientId),
                new Claim(ClaimsIdentity.DefaultNameClaimType, info.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, info.UserRole),
                new Claim(ClaimTypes.Hash, clientHash)
            };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

    }
}