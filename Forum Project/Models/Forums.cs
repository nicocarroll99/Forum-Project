using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.Models
{
    public class Forums
    {
        [Key]
        [Required]
        public int ForumId { get; set; }

        [Required]
        public string ForumName { get; set; }
    }
}
