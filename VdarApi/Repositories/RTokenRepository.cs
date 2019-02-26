﻿using Microsoft.Extensions.DependencyInjection;
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
                _context.TokensPair.RemoveRange(
                    _context.TokensPair.Where(
                        z => z.FingerPrint.Equals(FingerPrint) && z.ClientId.Equals(ClientId)).ToList()
                );
                _context.SaveChanges();
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
            catch {
                return false;
            }
        }

        public bool RefreshToken(TokensPair token)
        {
            _context.TokensPair.Update(token);
            return _context.SaveChanges() > 0;
        }

        public TokensPair GetToken(string finger_print, string access_token, string refresh_token)
        {
            return _context.TokensPair.FirstOrDefault(
                    x =>    x.FingerPrint.Equals(finger_print) &&
                            x.RefreshToken.Equals(refresh_token) &&
                            x.AccessToken.Equals(access_token)
            );
        }
    }
}
