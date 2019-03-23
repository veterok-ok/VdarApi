using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TokenService
{
    public class AuthOptions
    {
        public const string ISSUER = "VdarApi"; // издатель токена
        public const string AUDIENCE = "http://localhost:50000/"; // потребитель токена
        const string KEY = "vdar.api!my.secret.key.HARD";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 30 секунд
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
