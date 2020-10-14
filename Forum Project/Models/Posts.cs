using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.Models
{
    public class Posts
    {
        [Key]
        [Required]
        public int PostId { get; set; }

        [Required]
        [ForeignKey("ThreadId")]
        public Threads ThreadIdFK { get; set; }

        [Required]
        public Posts ParentIdFK { get; set; }

        [Required]
        public int Children { get; set; }

        [Required]
        [ForeignKey("Id")]
        public ApplicationUser UserIdFK { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }
    }
}
