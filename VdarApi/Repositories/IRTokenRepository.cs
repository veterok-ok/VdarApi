using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public interface IRTokenRepository
    {
        bool AddToken(Tokens token);

        bool RefreshToken(Tokens token);

        void RemoveToken(string ClientId, string FingerPrint);

        Tokens GetToken(string finger_print, string access_token, string refresh_token);

    }
}
