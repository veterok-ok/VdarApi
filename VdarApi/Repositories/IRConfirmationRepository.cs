using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public interface IRConfirmationRepository
    {
        Task<int> GetCountAttemptConfirmationAsync(string KeyType);
        Task InsertConfirmationKeyAsync(ConfirmationKey model);
        Task<bool> CheckConfirmationKeyAsync(ConfirmationKey model);
    }
}
