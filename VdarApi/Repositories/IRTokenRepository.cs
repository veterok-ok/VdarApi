using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public interface IRTokenRepository
    {
        Task AddTokenAsync(Tokens token);

        Task RefreshTokenAsync(Tokens token);

        Task RemoveTokenAsync(int ClientId, string FingerPrint);

        Task<Tokens> GetTokenAsync(string finger_print, string access_token, string refresh_token);

    }
}
