using CoreSecurityPrototype.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreSecurityPrototype.Data
{
    public class AuthPrototypeContext : IdentityDbContext<ApplicationUser>
    {
        public AuthPrototypeContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Contact> Contact { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Contact>().HasData(
                new Contact { Id = new Guid("0BF0A853-A243-491A-BB68-09CC94C16369"), FirstName = "Gavin", LastName = "Roux" },
                new Contact { Id = new Guid("51D98396-F0E5-4EC2-866F-8F6E8F23043A"), FirstName = "Dean", LastName = "Cowell" },
                new Contact { Id = new Guid("3DCDE597-DF95-4EE7-91F4-015799C11F29"), FirstName = "Michelle", LastName = "Uys" }
            );
        }
    }
}
