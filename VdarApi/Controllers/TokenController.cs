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
        private IRUserRepository _userRP;

        public TokenController(IRTokenRepository tokenRepository, IRUserRepository userRepository)
        {
            this._tokenRP = tokenRepository;
            this._userRP = userRepository;
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
            User _user = _userRP.LoginUser(parameters.username, parameters.password);
            
            if (_user == null)
                return new ResponseData(904);

            // Удаляем токен пользователя (в разрезе браузера), на случай если он украден
            _tokenRP.RemoveToken(_user.Id, parameters.finger_print??"");

            //Формируем новый токен
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var token = new Tokens
            {
                ClientId = _user.Id,
                AccessToken = GetJwt(_user, _user.GetHashCode().ToString()),
                RefreshToken = refresh_token,
                UpdateHashSum = _user.GetHashCode().ToString(),
                FingerPrint = parameters.finger_print??"",
                LastRefreshDateUTC = DateTime.UtcNow,
                CreatedDateUTC = DateTime.UtcNow,
                UserAgent = parameters.user_agent,
                Location = parameters.location,
                IP = parameters.ip
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
            var token = _tokenRP.GetToken(parameters.finger_print ?? "", access_token, parameters.refresh_token);

            if (token == null)
                return new ResponseData(906);

            User _user = new User()
            {
                Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
            };

            string claims_hash = User.FindFirstValue(ClaimTypes.Hash);


            if (token.UpdateHashSum.Equals(claims_hash))
            {
                //подтягиваем данные из существующего JWT
                _user.Name = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            }
            else
            {
                //подтягиваем данные из БД
                _user = _userRP.GetUser(_user.Id);

                if (_user == null)
                    return new ResponseData(904);

                claims_hash = token.UpdateHashSum;
            }

            token.LastRefreshDateUTC = DateTime.UtcNow;
            token.AccessToken = GetJwt(_user, claims_hash);
            token.Location = parameters.location;
            token.IP = parameters.ip;
            token.UserAgent = parameters.user_agent;

            if (_tokenRP.RefreshToken(token))
                return new ResponseData(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
            else
                return new ResponseData(907);
        }


        private string GetJwt(User info, string clientHash)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, info.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, info.Name),
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