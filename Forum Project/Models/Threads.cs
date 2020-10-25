using Forum_Project.Context;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ThreadId { get; set; }

        [Required]
        [ForeignKey("ForumId")]
        public Forums Forum { get; set; }
        public string ForumId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public string ThreadTitle { get; set; }

        [Required]
        public string Subject { get; set; }
    }
}
