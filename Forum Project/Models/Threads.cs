using Forum_Project.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.Models
{
    public class Threads
    {
        [Key]
        [Required]
        public int ThreadId { get; set; }

        [Required]
        [ForeignKey("ForumId")]
        public Forums ForumIdFK { get; set; }

        [Required]
        [ForeignKey("Id")]
        public ApplicationUser UserIdFK { get; set; }

        [Required]
        public string Subject { get; set; }
    }
}
