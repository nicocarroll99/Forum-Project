using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Forum_Project.Context
{
    public class ForumDbContext : DbContext 
    { 
        DbSet<Users> Users { get; set; }

        public ForumDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
