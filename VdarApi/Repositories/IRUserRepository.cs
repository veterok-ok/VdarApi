using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;
using VdarApi.ViewModels;

namespace VdarApi.Repositories
{
    public interface IRUserRepository
    {
        User GetUser (int id);
        User LoginUser (string login, string password);
        Task<User> GetUserByPhoneAsync(string phone);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByLoginAsync(string login);
        Task InsertBlankUserAsync(User model);
        Task SetConfirmationPhoneAsync(User model);
    }
}
