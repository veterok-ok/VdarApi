using Contracts.Repository;
using Contracts;
using Entities.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.RequestModels;

namespace TokenService
{
    public class TokenGenerator : ITokenGenerator
    {
        private IRepositoryWrapper _repo;

        public TokenGenerator(IRepositoryWrapper wrapperRepository)
        {
            _repo = wrapperRepository;
        }

        async public Task<Token> GenerateJWTTokenAsync(User user, ClientParameters parameters)
        {
            // Удаляем токен пользователя (в разрезе браузера), на случай если он украден
            var notActualTokens = await _repo.Token.GetFailedTokensAsync(user.UserId, parameters.finger_print ?? "");
            if (notActualTokens.Count() > 0)
            {
                _repo.Token.Delete(notActualTokens);
                await _repo.Token.SaveAsync();
            }
            //Формируем новый токен
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var token = new Token
            {
                UserId = user.UserId,
                AccessToken = GetJWT(user, user.GetHashCode().ToString()),
                RefreshToken = refresh_token,
                UpdateHashSum = user.GetHashCode().ToString(),
                FingerPrint = parameters.finger_print ?? "",
                LastRefreshDateUTC = DateTime.UtcNow,
                CreatedDateUTC = DateTime.UtcNow,
                UserAgent = parameters.user_agent ?? "",
                Location = parameters.location ?? "",
                IP = parameters.ip ?? ""
            };

            //Добавляем токен в таблицу БД
            _repo.Token.Create(token);
            await _repo.Token.SaveAsync();

            return token;
        }

        public string GetJWT(User user, string userHash, IEnumerable<Claim> claims = null)
        {
            var now = DateTime.UtcNow;
            if (claims == null)
                claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim("isEmptyProfile", IsEmptyProfile(user).ToString()),
                    new Claim(ClaimTypes.Hash, userHash)
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

        private bool IsEmptyProfile(User user)
        {
            return String.IsNullOrEmpty(user.Name) ||
                   String.IsNullOrEmpty(user.SurName) ||
                   user.Birthday == null;
        }

    }
}
