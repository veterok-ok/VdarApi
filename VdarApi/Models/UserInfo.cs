using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public string ClientId { get; set; }

        public static IList<UserInfo> GetAllUsers()
        {
            return new List<UserInfo>()
            {
                new UserInfo {  ClientId="100", UserName="admin",Password="123", UserRole="Administrator" },
                new UserInfo {  ClientId="101", UserName="user",Password="123", UserRole="user" },
            };
        }
    }
}
