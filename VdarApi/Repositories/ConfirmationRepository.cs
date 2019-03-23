using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Contracts;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public class ConfirmationRepository : RepositoryBase<ConfirmationKey>, IConfirmationRepository
    {
        public ConfirmationRepository(VdarDbContext repositoryContext)
          : base(repositoryContext)
        {

        }

        async public Task<bool> CheckConfirmationKeyAsync(ConfirmationKey key) => 
            await RepositoryContext.ConfirmationKeys.AsNoTracking().AnyAsync(c =>
                    c.KeyType.Equals(key.KeyType, StringComparison.CurrentCultureIgnoreCase) &&
                    c.UserId.Equals(key.UserId) &&
                    c.Key.Equals(key.Key)
                );

        async public Task<ConfirmationKey> EnterConfirmationAsync(ConfirmationKey key) =>
            await RepositoryContext.ConfirmationKeys.SingleOrDefaultAsync(c =>
                   c.KeyType.Equals(key.KeyType, StringComparison.CurrentCultureIgnoreCase) &&
                   c.UserId.Equals(key.UserId) &&
                   c.Key.Equals(key.Key)
                );

        async public Task<int> GetCountAttemptConfirmationAsync(int userId, string confirmationType) =>
            await RepositoryContext.ConfirmationKeys.AsNoTracking().CountAsync(c =>
                 c.KeyType.Equals(confirmationType, StringComparison.CurrentCultureIgnoreCase) &&
                 c.UserId.Equals(userId));

        public void RemoveNotActualKeys(ConfirmationKey key)
        {
            var _result = RepositoryContext.ConfirmationKeys.Where(c =>
                          c.KeyType.Equals(key.KeyType, StringComparison.CurrentCultureIgnoreCase) &&
                          c.UserId.Equals(key.UserId));
            RepositoryContext.RemoveRange(_result);
        }
    }
}
