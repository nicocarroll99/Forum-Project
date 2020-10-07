using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.ViewModels
{
    public class EditRoleViewModel
    {

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

    }
}
