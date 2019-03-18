using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Contracts;
using VdarApi.Models;
using VdarApi.ViewModels;

namespace VdarApi.Services
{
    public interface ITokenGenerator
    {
        Task<Tokens> GenerateJWTTokenAsync(User user, ITokenRepository _repo, ClientParameters parameter);
        string GetJWT(User user, string userHash);
    }
}
