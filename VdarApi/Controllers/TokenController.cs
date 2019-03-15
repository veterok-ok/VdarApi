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
    [Route("api/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IRTokenRepository _tokenRP;
        private IRUserRepository _userRP;

        public TokenController(IRTokenRepository tokenRepository, IRUserRepository userRepository)
        {
            this._tokenRP = tokenRepository;
            this._userRP = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<TokenResult>> Token([FromQuery]Parameters parameters)
        {
            if (parameters == null)
                return new TokenResult(901);

            switch (parameters.grant_type)
            {
                case "password":
                        return await DoAuthenticationAsync(parameters);
                case "refresh_token":
                    if (User.Identity.IsAuthenticated)
                        return await DoRefreshTokenAsync(parameters);
                    else
                        return new TokenResult(903);
                default:
                    return new TokenResult(902);
            };           

        }

        private async Task<TokenResult> DoAuthenticationAsync(Parameters parameters)
        {
            User _user = await _userRP.LoginUserAsync(parameters.username, parameters.password);
            
            if (_user == null)
                return new TokenResult(904);

            // Удаляем токен пользователя (в разрезе браузера), на случай если он украден
            await _tokenRP.RemoveTokenAsync(_user.Id, parameters.finger_print??"");

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
            await _tokenRP.AddTokenAsync(token);

            return new TokenResult(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
        }

        private async Task<TokenResult> DoRefreshTokenAsync(Parameters parameters)
        {
            string access_token = ControllerContext.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var token = await _tokenRP.GetTokenAsync(parameters.finger_print ?? "", access_token, parameters.refresh_token);

            if (token == null)
                return new TokenResult(906);

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
                _user = await _userRP.GetUserAsync(_user.Id);

                if (_user == null)
                    return new TokenResult(904);

                claims_hash = token.UpdateHashSum;
            }

            token.LastRefreshDateUTC = DateTime.UtcNow;
            token.AccessToken = GetJwt(_user, claims_hash);
            token.Location = parameters.location;
            token.IP = parameters.ip;
            token.UserAgent = parameters.user_agent;

            await _tokenRP.RefreshTokenAsync(token);

            return new TokenResult(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
            
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