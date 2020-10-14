using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum_Project.Migrations;
using Forum_Project.Models;
using Forum_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forum_Project.Controllers
{
    public class ForumController : Controller
    {
        private readonly ForumService forumService;

        public ForumController(ForumService service)
        {
            this.forumService = service;
        }

        public ForumService ForumService { get; }

        [HttpGet]
        public IActionResult AddForum()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddForum(Forums model)
        {
            forumService.AddForum(model);
            return View();
        }
    }
}
