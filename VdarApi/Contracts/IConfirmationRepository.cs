using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Contracts
{
    public interface IConfirmationRepository: IRepositoryBase<ConfirmationKey>
    {
        Task<ConfirmationKey> EnterConfirmationAsync(ConfirmationKey key);
        Task<bool> CheckConfirmationKeyAsync(ConfirmationKey key);
        Task<int> GetCountAttemptConfirmationAsync(int userId, string confirmationType);
        void RemoveNotActualKeys(ConfirmationKey key);
    }
}
