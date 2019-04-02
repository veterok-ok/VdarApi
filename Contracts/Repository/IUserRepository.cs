using System.Threading.Tasks;
using Entities.Models;

namespace Contracts.Repository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByPhoneAsync(string phone);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> LoginUserAsync(string login, string password);
    }
}
