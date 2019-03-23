using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repository
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
