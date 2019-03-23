using Entities.Models;
using Entities.RequestModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITokenGenerator
    {
        Task<Tokens> GenerateJWTTokenAsync(User user, ClientParameters parameter);
        string GetJWT(User user, string userHash, IEnumerable<Claim> claims);
    }
}
