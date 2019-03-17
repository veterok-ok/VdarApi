using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Contracts;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(VdarDbContext repositoryContext) 
            : base(repositoryContext)
        {

        }

        async public Task<User> GetUserByPhoneAsync(string phone) =>
            await RepositoryContext.Users.SingleOrDefaultAsync(u => 
                u.PhoneNumber.Equals(phone));

        async public Task<User> LoginUserAsync(string login, string password) =>
            await RepositoryContext.Users.SingleOrDefaultAsync(u =>
                u.Password.Equals(password) &&
                             (
                                u.Login.Equals(login) ||
                                u.Email.Equals(login) ||
                                u.PhoneNumber.Equals(login)
                              )
            );

        async public Task<User> GetUserByIdAsync(int id) =>
            await RepositoryContext.Users.SingleOrDefaultAsync(u =>
                u.Id.Equals(id));

    }
}
