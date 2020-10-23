using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.Models
{
    public class Forums
    {
        [Key]
        [Required]
        public Guid ForumId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        [Required]
        public string ForumName { get; set; }
    }
}
