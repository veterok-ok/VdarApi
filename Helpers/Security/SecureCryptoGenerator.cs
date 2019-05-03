using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers.Security
{
    public sealed class SecureCryptoGenerator
    {

        public static string GenerateUri(){

            byte[] linkBytes = new byte[96];
            var rngCrypto = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rngCrypto.GetBytes(linkBytes);
            var text128 = Convert.ToBase64String(linkBytes);

            return text128;
        }

    }
}
