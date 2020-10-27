using Forum_Project.Models;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.ViewModels
{
    public class ThreadViewModel
    {
        public string ThreadId { get; set; }
        public string ThreadTitle { get; set; }
        public string ForumId { get; set; }
        public string ForumName { get; set; }
        public string AuthorName { get; set; }
        public string UserId { get; set; }
        public DateTime PostedOn { get; set; }
        public string Subject { get; set; }
        public List<TreeExtensions.ITree<Posts>> Posts { get; set; }
    }
}
