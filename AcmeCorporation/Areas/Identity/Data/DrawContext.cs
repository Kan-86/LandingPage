using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcmeCorporation.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Data
{
    public class DrawContext : IdentityDbContext<User>
    {
        public DrawContext(DbContextOptions<DrawContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<UserInDraw> UserInDraw { get; set; }

        public DbSet<UserProfile> UserProfile { get; set; }

        public DbSet<Product> Product { get; set; }
        public DbSet<ProductSerialNumbers> ProductSerialNumber { get; set; }

    }
}
