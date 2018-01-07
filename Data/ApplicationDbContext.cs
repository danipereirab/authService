using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using AuthService.Models.ProfileViewModels;

namespace AuthService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            
            builder.Entity<IdentityRoleClaim<string>>().ToTable(Database.IsSqlServer() ? "RoleClaims" : "OtherName");
            builder.Entity<IdentityUserClaim<string>>().ToTable(Database.IsSqlServer() ? "UserClaims" : "OtherName"); 
            builder.Entity<IdentityUserLogin<string>>().ToTable(Database.IsSqlServer() ? "UserLogins" : "OtherName");
            builder.Entity<IdentityUserToken<string>>().ToTable(Database.IsSqlServer() ? "UserTokens" : "OtherName");
            builder.Entity<ApplicationUser>().ToTable(Database.IsSqlServer() ? "Users" : "OtherName");
            builder.Entity<ApplicationRole>().ToTable(Database.IsSqlServer() ? "Roles" : "OtherName");
            builder.Entity<IdentityUserRole<string>>().ToTable(Database.IsSqlServer() ? "UserRoles" : "OtherName");

            builder.Entity<ProfileViewModel>().ToTable(Database.IsSqlServer() ? "Profiles" : "OtherName").HasKey(k => k.Id);
            
            builder.Entity<ProfileViewModel>().HasOne(ho => ho.User).WithOne();


        }

    }
}
