using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VdarApi.Models;

namespace VdarApi.Repositories
{
    public class RTokenRepository : IRTokenRepository
    {
        VdarDbContext _context;
        public RTokenRepository(VdarDbContext context)
        {
            this._context = context;
        }

        async public Task RemoveTokenAsync(int ClientId, string FingerPrint)
        {
            var _list = await _context.Tokens.Where(
                           z => z.FingerPrint.Equals(FingerPrint) && z.ClientId.Equals(ClientId)).ToListAsync();
            
            if (_list.Count() > 0) { 
                    _context.Tokens.RemoveRange(_list);
                    await _context.SaveChangesAsync();
                }
        }
        
        async public Task AddTokenAsync(Tokens token)            
        {
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
        }

        async public Task RefreshTokenAsync(Tokens token)
        {
            _context.Tokens.Update(token);
             await _context.SaveChangesAsync();
        }

        async public Task<Tokens> GetTokenAsync(string finger_print, string access_token, string refresh_token) =>
         await _context.Tokens.FirstOrDefaultAsync(
                        x =>    x.FingerPrint.Equals(finger_print) &&
                                x.RefreshToken.Equals(refresh_token) &&
                                x.AccessToken.Equals(access_token)
                );        
    }
}
