using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.ViewModels
{
    public class ForumViewModel
    {
        public string ForumId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string ForumName { get; set; }
        public string AuthorName { get; set; }
        public DateTime PostedOn { get; set; }
        public List<ThreadViewModel> Threads { get; set; }
    }
}
