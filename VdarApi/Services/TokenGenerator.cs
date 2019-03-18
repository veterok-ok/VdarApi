﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VdarApi.Contracts;
using VdarApi.Models;
using VdarApi.ViewModels;

namespace VdarApi.Services
{
    public class TokenGenerator : ITokenGenerator
    {
        async public Task<Tokens> GenerateJWTTokenAsync(User user, ITokenRepository _repo, ClientParameters parameters)
        {
            // Удаляем токен пользователя (в разрезе браузера), на случай если он украден
            var notActualTokens = await _repo.GetFailedTokensAsync(user.Id, parameters.finger_print ?? "");
            if (notActualTokens.Count() > 0)
            {
                _repo.Delete(notActualTokens);
                await _repo.SaveAsync();
            }
            //Формируем новый токен
            var refresh_token = Guid.NewGuid().ToString().Replace("-", "");
            var token = new Tokens
            {
                ClientId = user.Id,
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
            _repo.Create(token);
            await _repo.SaveAsync();

            return token;
        }

        public string GetJWT(User user, string userHash)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name?? ""),
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
    }
}