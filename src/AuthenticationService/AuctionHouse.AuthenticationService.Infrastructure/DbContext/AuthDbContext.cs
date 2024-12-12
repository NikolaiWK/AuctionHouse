using AuctionHouse.AuthenticationService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;




namespace AuctionHouse.AuthenticationService.Infrastructure.DbContext
{
    public class AuthDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(20);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(50);
                entity.HasOne(u => u.Address).WithOne().HasForeignKey<UserAddress>(a => a.Id).OnDelete(DeleteBehavior.Cascade); //If a user is deleted it deletes the related adress too

            });


        }
    }


}


