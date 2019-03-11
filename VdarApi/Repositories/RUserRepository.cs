using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;
using VdarApi.ViewModels;

namespace VdarApi.Repositories
{
    public class RUserRepository : IRUserRepository
    {
        VdarDbContext _context;
        public RUserRepository(VdarDbContext context)
        {
            this._context = context;
        }

        public User GetUser(int id)
        {
            try
            {
                return _context.Users.FirstOrDefault(
                        x => x.Id.Equals(id)
                );
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User LoginUser(string login, string password)
        {
            try
            {
                return _context.Users.FirstOrDefault(
                        x => x.Password.Equals(password) && 
                             (
                                x.Login.Equals(login) || 
                                x.Email.Equals(login) || 
                                x.PhoneNumber.Equals(login)
                              )
                );
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        async public Task<User> GetUserByPhoneAsync(string phone) => 
            await _context.Users.SingleOrDefaultAsync(z => z.PhoneNumber.Equals(phone));

        async public Task InsertBlankUserAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
