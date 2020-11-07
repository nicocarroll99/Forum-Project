using Forum_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.ViewModels
{
    public class PostViewModel
    {
        public string PostId { get; set; }
        public string ThreadId { get; set; }
        public string ForumId { get; set; }
        public string ParentId { get; set; }
        public string UserId { get; set; }
        public string AuthorName { get; set; }
        public int ChildrenCount { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime postedOn { get; set; }
        public List<Posts> Children { get; set; }
    }
}
