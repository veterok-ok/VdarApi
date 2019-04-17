using Entities.Models;
using System.Threading.Tasks;

namespace Contracts.Repository
{
    public interface IConfirmationRepository: IRepositoryBase<ConfirmationKey>
    {
        Task<ConfirmationKey> EnterConfirmationAsync(ConfirmationKey key);
        Task<ConfirmationKey> GetByUri(string uri);
        Task<ConfirmationKey> GetByHashCode(string hashcode, string type);
        Task<bool> CheckConfirmationKeyAsync(ConfirmationKey key);
        Task<int> GetCountAttemptConfirmationAsync(int userId, string confirmationType);
        void RemoveNotActualKeys(ConfirmationKey key);
    }
}
