using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        //Seed Some Data
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // define many to many relationship
            // Portfolio has 2 primary key 
            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            // one to many relationship user to portfolios
            builder.Entity<Portfolio>()
                .HasOne(x => x.AppUser)
                .WithMany(x => x.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            // one-to-many stock to portfolios
            builder.Entity<Portfolio>()
                .HasOne(x => x.Stock)
                .WithMany(x => x.portfolios)
                .HasForeignKey(p => p.StockId);

            //bind product and order relationship
            builder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);


            // define role for role table when data migrate
            List<IdentityRole> roles = new List<IdentityRole>(){
                new IdentityRole{
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole{
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            // seeded roles data into IdentityRole table
            builder.Entity<IdentityRole>().HasData(roles);

            var adminUser = new AppUser
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<AppUser>().HashPassword(null, "Admin@123"),
                SecurityStamp = string.Empty
            };
            builder.Entity<AppUser>().HasData(adminUser);

            // assign the admin user to the Admin role
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roles[0].Id,
                UserId = adminUser.Id
            });

        }
    }
}