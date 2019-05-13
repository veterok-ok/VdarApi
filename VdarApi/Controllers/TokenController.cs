using Entities.Models;
using Entities.RequestModels;
using Entities.ResponseModels;
using Contracts;
using Contracts.Repository;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Helpers.Security;
using Microsoft.AspNetCore.Authorization;

namespace VdarApi.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IRepositoryWrapper _repo;
        private ITokenGenerator tokenGenerator;

        public TokenController(IRepositoryWrapper wrapperRepository, ITokenGenerator tokenGenerator)
        {
            this._repo = wrapperRepository;
            this.tokenGenerator = tokenGenerator;
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
            var _user = await _repo.User.LoginUserAsync(parameters.username, parameters.password);

            if (_user == null || !SecurePasswordHasherHelper.Validate(parameters.password, _user.Salt, _user.Password))
                return new TokenResult(904);

            var token = await tokenGenerator.GenerateJWTTokenAsync(_user, (ClientParameters)parameters);

            return new TokenResult(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
        }

        private async Task<TokenResult> DoRefreshTokenAsync(Parameters parameters)
        {
            string access_token = ControllerContext.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            var token = await _repo.Token.GetTokenAsync(parameters.finger_print ?? "", access_token, parameters.refresh_token);

            if (token == null)
                return new TokenResult(906);

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string claims_hash = User.FindFirstValue(ClaimTypes.Hash);

            User _user = null;
            if (!token.UpdateHashSum.Equals(claims_hash))
            {
                //подтягиваем данные из БД
                _user = await _repo.User.GetUserByIdAsync(userId);

                if (_user == null)
                    return new TokenResult(904);

                claims_hash = token.UpdateHashSum;
            }

            token.LastRefreshDateUTC = DateTime.UtcNow;
            token.AccessToken = tokenGenerator.GetJWT(_user, claims_hash, User.Claims);
            token.Location = parameters.location;
            token.IP = parameters.ip;
            token.UserAgent = parameters.user_agent;

             _repo.Token.Update(token);
            await _repo.Token.SaveAsync();

            return new TokenResult(999, new
                {
                    access_token = token.AccessToken,
                    refresh_token = token.RefreshToken
                });
            
        }

    }
}