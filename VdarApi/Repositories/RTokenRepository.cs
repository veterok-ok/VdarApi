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

        public void RemoveToken(int ClientId, string FingerPrint)
        {
            try
            {
                var _list = _context.Tokens.Where(
                           z => z.FingerPrint.Equals(FingerPrint) && z.ClientId.Equals(ClientId));
                if (_list.Count() > 0) { 
                    _context.Tokens.RemoveRange(_list);
                    _context.SaveChanges();
                }
            }
            catch {  }
        }


        public bool AddToken(Tokens token)
        {
            try
            {
                _context.Tokens.Add(token);
                return _context.SaveChanges() > 0;
            }
            catch(Exception ex) {
                return false;
            }
        }

        public bool RefreshToken(Tokens token)
        {
            try
            {
                _context.Tokens.Update(token);
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Tokens GetToken(string finger_print, string access_token, string refresh_token)
        {
            try { 
                return _context.Tokens.FirstOrDefault(
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
