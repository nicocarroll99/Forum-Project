﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
