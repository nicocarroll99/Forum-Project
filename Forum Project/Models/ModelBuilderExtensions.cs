using Forum_Project.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Forum_Project.Models
{
    public static class ModelBuilderExtensions
    {
        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                });
        }

        public static void BuildForumDB(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Posts>()
                .Property(b => b.Children).HasDefaultValue(0);

            //modelBuilder.Entity<Threads>()
            //    .HasOne(e => e.ForumIdFK)
            //    .WithOne();

            //modelBuilder.Entity<Threads>()
            //    .HasOne(e => e.UserIdFK)
            //    .WithOne();

            //modelBuilder.Entity<Posts>()
            //    .HasOne(e => e.ThreadIdFK)
            //    .WithOne();

            //modelBuilder.Entity<Posts>()
            //    .HasOne(e => e.ParentIdFK)
            //    .WithOne();

            //modelBuilder.Entity<Posts>()
            //    .HasOne(e => e.UserIdFK)
            //    .WithOne();
                
        }
    }
}
