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

        async public Task<User> GetUserAsync(int id) =>
            await _context.Users.FirstOrDefaultAsync(
                        x => x.Id.Equals(id)
                );
        

        async public Task<User> LoginUserAsync(string login, string password) =>
            await _context.Users.FirstOrDefaultAsync(
                        x => x.Password.Equals(password) &&
                             (
                                x.Login.Equals(login) ||
                                x.Email.Equals(login) ||
                                x.PhoneNumber.Equals(login)
                              )
                );
            

        async public Task<User> GetUserByLoginAsync(string login) =>
            await _context.Users.SingleOrDefaultAsync(z => z.Login.Equals(login));

        async public Task<User> GetUserByEmailAsync(string email) =>
            await _context.Users.SingleOrDefaultAsync(z => z.Email.Equals(email));

        async public Task<User> GetUserByPhoneAsync(string phone) =>
            await _context.Users.SingleOrDefaultAsync(z => z.PhoneNumber.Equals(phone));

        async public Task InsertBlankUserAsync(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        async public Task SetConfirmationPhoneAsync(User user){
            user.ActivatedDateUtc = DateTime.UtcNow;
            user.PhoneIsConfirmed = true;
            user.IsActive = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
