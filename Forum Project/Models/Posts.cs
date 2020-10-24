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
        public Guid PostId { get; set; }

        [Required]
        [ForeignKey("ForumId")]
        public Forums Forum { get; set; }
        public Guid ForumId { get; set; }

        [Required]
        [ForeignKey("ThreadId")]
        public Threads Thread { get; set; }
        public Guid ThreadId { get; set; }

        [Required]
        [ForeignKey("ParentId")]
        public Posts Parent { get; set; }
        public Guid ParentId { get; set; }
        public ICollection<Posts> ChildrenPosts { get; } = new List<Posts>();

        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public int Children { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime PostedOn { get; set; }
    }
}
