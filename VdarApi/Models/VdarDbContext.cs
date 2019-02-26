using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VdarApi.Models
{
    public class VdarDbContext : DbContext
    {
        public VdarDbContext(DbContextOptions<VdarDbContext> options)
        : base(options)
        {

        }

        public DbSet<TokensPair> TokensPair { get; set; }
      
    }
}
