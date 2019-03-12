using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public class RConfirmationRepository : IRConfirmationRepository
    {
        private VdarDbContext _context;

        public RConfirmationRepository(VdarDbContext context)
        {
            this._context = context;
        }

        async public Task<bool> CheckConfirmationKeyAsync(ConfirmationKey model)
        => await _context.ConfirmationKeys.AsNoTracking().AnyAsync(c =>
                   c.KeyType.Equals(model.KeyType, StringComparison.CurrentCultureIgnoreCase) &&
                   c.UserId.Equals(model.UserId) &&
                   c.Key.Equals(model.Key)
                );
        

        async public Task<int> GetCountAttemptConfirmationAsync(string KeyType) => 
            await _context.ConfirmationKeys.AsNoTracking().CountAsync(c => 
                 c.KeyType.Equals(KeyType, StringComparison.CurrentCultureIgnoreCase));

        async public Task InsertConfirmationKeyAsync(ConfirmationKey model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
        }
    }
}
