using Forum_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.ViewModels
{
    public class PostViewModel
    {
        public string PostId { get; set; }
        public string ThreadId { get; set; }
        public string ParentId { get; set; }
        public string UserId { get; set; }
        public int ChildrenCount { get; set; }
        public string Message { get; set; }
        public DateTime postedOn { get; set; }
        public List<Posts> Children { get; set; }
    }
}
