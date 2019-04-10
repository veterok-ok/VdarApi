using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repository
{
    public interface ITokenRepository: IRepositoryBase<Token>
    {
        Task<List<Token>> GetFailedTokensAsync(int userId, string fingerPrint);
        Task<Token> GetTokenAsync(string fingerPrint, string accessToken, string refreshToken);
    }
}
