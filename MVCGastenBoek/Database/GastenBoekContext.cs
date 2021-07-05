using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCGastenBoek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCGastenBoek.Database
{
    public class GastenBoekContext : IdentityDbContext<ApplicationUser>
    {
        public GastenBoekContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Comment> Comments { get; set; }

    }
}
