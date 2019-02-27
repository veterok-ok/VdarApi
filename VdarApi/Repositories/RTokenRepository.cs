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

        public void RemoveToken(string ClientId, string FingerPrint)
        {
            try
            {
                var _list = _context.TokensPair.Where(
                           z => z.FingerPrint.Equals(FingerPrint) && z.ClientId.Equals(ClientId));
                if (_list.Count() > 0) { 
                    _context.TokensPair.RemoveRange(_list);
                    _context.SaveChanges();
                }
            }
            catch {  }
        }


        public bool AddToken(TokensPair token)
        {
            try
            {
                _context.TokensPair.Add(token);
                return _context.SaveChanges() > 0;
            }
            catch(Exception ex) {
                return false;
            }
        }

        public bool RefreshToken(TokensPair token)
        {
            try
            {
                _context.TokensPair.Update(token);
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public TokensPair GetToken(string finger_print, string access_token, string refresh_token)
        {
            try { 
                return _context.TokensPair.FirstOrDefault(
                        x =>    x.FingerPrint.Equals(finger_print) &&
                                x.RefreshToken.Equals(refresh_token) &&
                                x.AccessToken.Equals(access_token)
                );
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
