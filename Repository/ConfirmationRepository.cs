using Contracts.Repository;
using Entities;
using Entities.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Repository
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
            await RepositoryContext.ConfirmationKeys.FirstOrDefaultAsync(c =>
                   c.KeyType.Equals(key.KeyType, StringComparison.CurrentCultureIgnoreCase) &&
                   c.UserId.Equals(key.UserId) &&
                   c.Key.Equals(key.Key)
                );

        async public Task<ConfirmationKey> GetByUri(string uri) =>
            await RepositoryContext.ConfirmationKeys.SingleOrDefaultAsync(c =>
                   c.HashCode.Equals(uri)
                );

        async public Task<ConfirmationKey> GetByHashCode(string hashcode, string type) =>
            await RepositoryContext.ConfirmationKeys.SingleOrDefaultAsync(c =>
                   c.HashCode.Equals(hashcode) &&
                   c.KeyType.Equals(type)
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
