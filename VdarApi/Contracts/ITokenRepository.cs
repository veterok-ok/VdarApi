using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Contracts
{
    public interface ITokenRepository: IRepositoryBase<Tokens>
    {
        Task<List<Tokens>> GetFailedTokensAsync(int userId, string fingerPrint);
        Task<Tokens> GetTokenAsync(string fingerPrint, string accessToken, string refreshToken);
    }
}
