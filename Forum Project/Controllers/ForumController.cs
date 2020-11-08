using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum_Project.Migrations;
using Forum_Project.Models;
using Forum_Project.Services;
using Forum_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> AddForum(ForumViewModel model)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                model.UserId = user.Id;
                model.AuthorName = user.UserName;

                await forumService.AddForum(model);
            }
            catch
            {
                ViewBag.ErrorTitle = "Adding Forum Error";
                ViewBag.ErrorMessage = "There was an issue adding your Forum. Please contact us for support.";
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteForum(string forumId)
        {
            try
            {
                var forum = forumService.GetForum(forumId);

                if (userManager.GetUserId(User) == forum.UserId || User.IsInRole("Admin"))
                {
                    await forumService.DeleteForum(forumId);
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Deleting Forum Error";
                ViewBag.ErrorMessage = "There was an issue deleting your forum. Please contact us for support.";
                return View("Error");
            }


            return RedirectToAction("Index", "Home");
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
            try
            {
                var user = await userManager.GetUserAsync(User);
                model.UserId = user.Id;
                model.AuthorName = user.UserName;

                await forumService.AddThread(model);
            }
            catch
            {
                ViewBag.ErrorTitle = "Adding Thread Error";
                ViewBag.ErrorMessage = "There was an issue adding your thread. Please contact us for support.";
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteThread(string threadId)
        {
            try
            {
                var thread = forumService.GetThread(threadId);

                if (userManager.GetUserId(User) == thread.UserId || User.IsInRole("Admin"))
                {
                    await forumService.DeleteThread(threadId);
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Deleting Thread Error";
                ViewBag.ErrorMessage = "There was an issue deleting your thread. Please contact us for support.";
                return View("Error");
            }


            return RedirectToAction("Index", "Home");
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
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.GetUserAsync(User);
                    model.UserId = user.Id;
                    model.AuthorName = user.UserName;

                    await forumService.AddPost(model);
                }
                catch
                {
                    ViewBag.ErrorTitle = "Adding Post Error";
                    ViewBag.ErrorMessage = "There was an issue adding your post. Please contact us for support.";
                    return View("Error");
                }
            }

            return RedirectToAction("ThreadPosts", new { threadId = model.ThreadId });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(PostViewModel model)
        {
            try
            {
                var post = forumService.GetPost(model.PostId).Result;
                // Check on server side if the user is the owner of the post
                if (userManager.GetUserId(User) == post.UserId || User.IsInRole("Admin"))
                {
                    await forumService.DeletePost(post);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Deleting Error";
                ViewBag.ErrorMessage = "There was an issue deleting your post. Please contact us for support.";
                return View("Error");
            }

            return RedirectToAction("ThreadPosts", new { threadId = model.ThreadId });
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(PostViewModel model)
        {
            try
            {
                var post = forumService.GetPost(model.PostId).Result;

                if (userManager.GetUserId(User) == post.UserId || User.IsInRole("Admin"))
                {
                    post.Message = model.Message;

                    await forumService.EditPost(post);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Editing Error";
                ViewBag.ErrorMessage = "There was an issue editing your post. Please contact us for support.";
                return View("Error");
            }
            return RedirectToAction("ThreadPosts", new { threadId = model.ThreadId });
        }

        [HttpPost]
        public async Task<IActionResult> EditThread(ThreadViewModel model)
        {
            try
            {
                var thread = forumService.GetThreadModel(model.ThreadId);

                if (userManager.GetUserId(User) == thread.UserId || User.IsInRole("Admin"))
                {
                    thread.ThreadTitle = model.ThreadTitle;
                    thread.Subject = model.Subject;

                    await forumService.EditThread(thread);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Editing Error";
                ViewBag.ErrorMessage = "There was an issue editing your thread. Please contact us for support.";
                return View("Error");
            }
            return RedirectToAction("ThreadPosts", new { threadId = model.ThreadId });
        }

        [HttpPost]
        public async Task<IActionResult> EditForum(ForumViewModel model)
        {
            try
            {
                var forum = forumService.GetForumModel(model.ForumId);

                if (userManager.GetUserId(User) == forum.UserId || User.IsInRole("Admin"))
                {
                    forum.ForumName = model.ForumName;

                    await forumService.EditForum(forum);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Editing Error";
                ViewBag.ErrorMessage = "There was an issue editing your post. Please contact us for support.";
                return View("Error");
            }
            return RedirectToAction("Index", "Home");
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
