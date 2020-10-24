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
        public async Task<IActionResult> AddForum(Forums model)
        {
            await forumService.AddForum(model);
            return View();
        }

        // Need to test everything below this point: Database may be screwed up
        [HttpGet]
        public IActionResult AddThread()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(int forumId, Threads model)
        {
            await forumService.AddThread(forumId, model);
            return View();
        }
    }
}
