using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Forum_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Forum_Project.Context
{
    public class ForumDbContext : IdentityDbContext<ApplicationUser>
    {
        public ForumDbContext(DbContextOptions options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);     
        }
    }

    
}
