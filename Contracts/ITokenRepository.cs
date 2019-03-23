using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITokenRepository: IRepositoryBase<Tokens>
    {
        Task<List<Tokens>> GetFailedTokensAsync(int userId, string fingerPrint);
        Task<Tokens> GetTokenAsync(string fingerPrint, string accessToken, string refreshToken);
    }
}
