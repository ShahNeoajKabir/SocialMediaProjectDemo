using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.ModelClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Data
{
    public class DatabaseContext : IdentityDbContext<AppUser,AppRole,int,IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,
        IdentityRoleClaim<int>,IdentityUserToken<int>
        >
    {
        public DatabaseContext( DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<UserLike> Likes { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany(p => p.UserRole)
                .WithOne(p => p.User)
                .HasForeignKey(f => f.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
               .HasMany(p => p.UserRole)
               .WithOne(p => p.Role)
               .HasForeignKey(f => f.RoleId)
               .IsRequired();

            //Seed Data
            //modelBuilder.Entity<AppRole>().HasData(
            //    new AppRole { Id = 1, Name = "Admin" },
            //    new AppRole { Id = 2, Name = "Moderator" },
            //    new AppRole { Id = 3, Name = "Member" }
            //);


            modelBuilder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            modelBuilder.Entity<UserLike>()
                .HasOne(o => o.SourceUser)
                .WithMany(p => p.LikedUsers)
                .HasForeignKey(f => f.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserLike>()
               .HasOne(o => o.LikedUser)
               .WithMany(p => p.LikedByUsers)
               .HasForeignKey(f => f.LikedUserId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
