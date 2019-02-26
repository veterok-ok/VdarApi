using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public interface IRTokenRepository
    {
        bool AddToken(TokensPair token);

        bool RefreshToken(TokensPair token);

        void RemoveToken(string ClientId, string FingerPrint);

        TokensPair GetToken(string finger_print, string access_token, string refresh_token);

    }
}
