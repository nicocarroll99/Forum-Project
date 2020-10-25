using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum_Project.Migrations;
using Forum_Project.Models;
using Forum_Project.Services;
using Forum_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum_Project.Controllers
{
    public class ForumController : Controller
    {
        private readonly ForumService forumService;
        private readonly UserManager<ApplicationUser> userManager;

        public ForumController(ForumService service, UserManager<ApplicationUser> userManager)
        {
            this.forumService = service;
            this.userManager = userManager;
        }

        public ForumService ForumService { get; }

        [HttpGet]
        public IActionResult AddForum()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(ForumViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            model.UserId = user.Id;
            model.AuthorName = user.UserName;

            await forumService.AddForum(model);
            return View();
        }

        [HttpGet]
        public IActionResult AddThread(string forumId)
        {
            ThreadViewModel threadViewModel = new ThreadViewModel()
            {
                ForumId = forumId
            };

            return View(threadViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddThread(ThreadViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            model.UserId = user.Id;
            model.AuthorName = user.UserName;

            await forumService.AddThread(model);
            return View();
        }

        [HttpGet]
        public IActionResult AddPost(string threadId, string forumId)
        {
            PostViewModel postViewModel = new PostViewModel
            {
                ThreadId = threadId,
                ForumId = forumId
            };

            return View(postViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(PostViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            model.UserId = user.Id;
            model.AuthorName = user.UserName;

            await forumService.AddPost(model);
            return View();
        }

        [HttpGet]
        public IActionResult ThreadPosts(string threadId)
        {
            var thread = forumService.GetThread(threadId);
            var posts = forumService.GetThreadPosts(threadId);

            thread.Posts = posts;

            return View(thread);
        }
    }
}
