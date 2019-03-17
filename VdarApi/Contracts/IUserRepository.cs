using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Contracts
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByPhoneAsync(string phone);
        Task<User> LoginUserAsync(string login, string password);
    }
}
