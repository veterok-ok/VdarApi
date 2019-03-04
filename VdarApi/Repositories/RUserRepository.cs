using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public class RUserRepository : IRUserRepository
    {
        VdarDbContext _context;
        public RUserRepository(VdarDbContext context)
        {
            this._context = context;
        }

        public User GetUser(string id)
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

        async public Task<bool> UserExistAsync(string phone)
        {
            return await _context.Users.AnyAsync(z => z.PhoneNumber.Equals(phone));
        }

    }
}
