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
        /* Расшифровка Code
         * 901 - Нет данных в запросе [FromQuery]
         * 902 - Указанный пользователь не найдет
         * 903 - Пользователь не аутентифицирован (отсутствует JWT в заголовке)
         * 904 - Неверный Grand_Type
         * 905 - Не найден Token обновления в запросе
         * 906 - Не удалось обновить токен в БД
         * 909 - Не удалось добавить новый токен в БД
         * 910 - Не удалось обновить токен в БД
         * 999 - Всё ок
         */
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
                if (User.Identity.IsAuthenticated)
                {
                    await Response.WriteAsync(JsonConvert.SerializeObject(DoRefreshToken(parameters), new JsonSerializerSettings { Formatting = Formatting.Indented }));
                }
                else
                {
                    var response = new ResponseData
                    {
                        Code = "903",
                        Message = "User Is Not Authenticated",
                        Data = null
                    };
                    await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
                }
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
            _tokenRP.RemoveToken(_user.ClientId, parameters.finger_print??"");

            //Формируем новый токен
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var TokenPair = new TokensPair
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = _user.ClientId,
                AccessToken = GetJwt(_user.ClientId, _user.UserName, _user.UserRole, _user.GetHashCode().ToString()),
                RefreshToken = refresh_token,
                UpdateHashSum = _user.GetHashCode().ToString(),
                FingerPrint = parameters.finger_print??"",
                CreatedDateUTC = DateTime.UtcNow
            };

            //Добавляем токен в таблицу БД
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
            string access_token = ControllerContext.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var token = _tokenRP.GetToken(parameters.finger_print??"", access_token, parameters.refresh_token);
            
            if (token == null)
            {
                return new ResponseData
                {
                    Code = "905",
                    Message = "Can not refresh token",
                    Data = null
                };
            }

            string claims_client_id, claims_name, claims_role, claims_hash;

            claims_hash = User.Claims.Where(c => c.Type == ClaimTypes.Hash)
                   .Select(c => c.Value).SingleOrDefault();

            claims_client_id = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();

            if (token.UpdateHashSum.Equals(claims_hash))
            {
                //подтягиваем данные из существующего JWT
                claims_name = User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType)
                       .Select(c => c.Value).SingleOrDefault();

                claims_role = User.Claims.Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)
                       .Select(c => c.Value).SingleOrDefault();
            }
            else
            {
                //подтягиваем данные из БД
                var _user = UserInfo.GetAllUsers()
                           .SingleOrDefault(z => z.ClientId.Equals(claims_client_id));

                if (_user == null)
                {
                    return new ResponseData
                    {
                        Code = "902",
                        Message = "invalid user infomation",
                        Data = null
                    };
                }
                claims_name = _user.UserName;
                claims_role = _user.UserRole;
                claims_hash = token.UpdateHashSum;
            }


            token.CreatedDateUTC = DateTime.UtcNow;
            token.AccessToken = GetJwt(claims_client_id, claims_name, claims_role, claims_hash);
            if (_tokenRP.RefreshToken(token))
            {
                return new ResponseData
                {
                    Code = "999",
                    Message = "OK",
                    Data = new
                    {
                        access_token = token.AccessToken,
                        refresh_token = token.RefreshToken
                    }
                };
            }
            else
            {
                return new ResponseData
                {
                    Code = "906",
                    Message = "can not add token to database",
                    Data = null
                };
            }
        }


        private string GetJwt(string clientID, string clientName, string clientRole, string clientHash)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, clientID),
                new Claim(ClaimsIdentity.DefaultNameClaimType, clientName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, clientRole),
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