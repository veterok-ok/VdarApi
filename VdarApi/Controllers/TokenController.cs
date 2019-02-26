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

        [HttpPost("/token")]
        public async Task Token([FromQuery]Parameters parameters)
        {
            Response.ContentType = "application/json";

            if (parameters == null)
            {
                var response = new ResponseData
                {
                    Code = "901",
                    Message = "No data",
                    Data = null
                };
                await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }

            if (parameters.grant_type == "password")
            {
                await Response.WriteAsync(JsonConvert.SerializeObject(DoAuthentication(parameters), new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            else if (parameters.grant_type == "refresh_token")
            {
                await Response.WriteAsync(JsonConvert.SerializeObject(DoRefreshToken(parameters), new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }
            else
            {
                var response = new ResponseData
                {
                    Code = "904",
                    Message = "Bad Request",
                    Data = null
                };                
                await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            }

        }

        private ResponseData DoAuthentication(Parameters parameters)
        {
            var _user = UserInfo.GetAllUsers()
                        .SingleOrDefault( z => z.Password.Equals(parameters.password) 
                                            && z.UserName.Equals(parameters.username));
                        

            if (_user == null)
            {
                return new ResponseData
                {
                    Code = "902",
                    Message = "invalid user infomation",
                    Data = null
                };
            }

            // Удаляем токен пользователя (в разрезе браузера), на случай если он украден
            _tokenRP.RemoveToken(_user.ClientId, parameters.finger_print);

            //Формируем новый токен
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var TokenPair = new TokensPair
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = _user.ClientId,                
                RefreshToken = refresh_token,
                AccessToken = GetJwt(_user),
                FingerPrint = parameters.finger_print,
                UpdateHashSum = _user.GetHashCode().ToString()
            };

            if (_tokenRP.AddToken(TokenPair))
            {
                return new ResponseData
                {
                    Code = "999",
                    Message = "OK",
                    Data = new {
                        access_token = TokenPair.AccessToken,
                        refresh_token = TokenPair.RefreshToken
                    }
                };
            }
            else
            {
                return new ResponseData
                {
                    Code = "909",
                    Message = "can not add token to database",
                    Data = null
                };
            }
        }

        private ResponseData DoRefreshToken(Parameters parameters)
        {
            var token = _tokenRP.GetToken(parameters.finger_print, parameters.access_token, parameters.refresh_token);

            if (token == null)
            {
                return new ResponseData
                {
                    Code = "905",
                    Message = "Can not refresh token",
                    Data = null
                };
            }

            return null;
            //if (token.UpdateHashSum == parameters.update_hash_sum)


           /* var _user = UserInfo.GetAllUsers()
                       .SingleOrDefault(z => token.ClientId.Equals(z.ClientId));*/


           /* var updateFlag = _tokenRP.RefreshToken(token);

            if (updateFlag)
            {
                return new ResponseData
                {
                    Code = "999",
                    Message = "OK",
                    Data = new
                    {
                        access_token = ""//TokenPair.AccessToken
                    }
                };
            }
            else
            {
                return new ResponseData
                {
                    Code = "910",
                    Message = "can not expire token or a new token",
                    Data = null
                };
            }*/
        }


        private string GetJwt(UserInfo client)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, client.ClientId),
                new Claim(ClaimsIdentity.DefaultNameClaimType, client.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, client.UserRole)
            };

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

    }
}