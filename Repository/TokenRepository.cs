﻿using Contracts.Repository;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TokenRepository : RepositoryBase<Tokens>, ITokenRepository
    {
        public TokenRepository(VdarDbContext repositoryContext)
          : base(repositoryContext)
        {

        }

        async public Task<List<Tokens>> GetFailedTokensAsync(int userId, string fingerPrint) =>
            await RepositoryContext.Tokens.Where(
                           z => z.FingerPrint.Equals(fingerPrint) && z.ClientId.Equals(userId)).ToListAsync();

        async public Task<Tokens> GetTokenAsync(string fingerPrint, string accessToken, string refreshToken) =>
            await RepositoryContext.Tokens.FirstOrDefaultAsync(
                        x =>    x.FingerPrint.Equals(fingerPrint) &&
                                x.RefreshToken.Equals(refreshToken) &&
                                x.AccessToken.Equals(accessToken)
                );
    }
}
