using Forum_Project.Context;
using Forum_Project.Migrations;
using Forum_Project.Models;
using Forum_Project.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Forum_Project.Services
{
    public class ForumService
    {
        private readonly ForumDbContext forumDbContext;

        public ForumService(ForumDbContext context)
        {
            forumDbContext = context;
        }

        public async Task AddForum(ForumViewModel model)
        {
            var newForum = new Forums
            {
                ForumName = model.ForumName,
                AuthorName = model.AuthorName,
                UserId = model.UserId
            };

            forumDbContext.Forums.Add(newForum);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task AddThread(ThreadViewModel model)
        {
            var newThread = new Threads
            {
                ForumId = model.ForumId,
                ThreadTitle = model.ThreadTitle,
                AuthorName = model.AuthorName,
                PostedOn = DateTime.Now,
                Subject = model.Subject,
                UserId = model.UserId
            };

            await forumDbContext.Threads.AddAsync(newThread);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task AddPost(PostViewModel model)
        {
            var newPost = new Posts
            {
                ThreadId = model.ThreadId,
                Message = model.Message,
                AuthorName = model.AuthorName,
                ForumId = model.ForumId,
                ParentId = model.ParentId,
                PostedOn = DateTime.Now,
                UserId = model.UserId,
                Children = 0
            };

            // Find the parent post and increment the child count
            if (model.ParentId != null)
            {
                var parentPost = forumDbContext.Posts
                    .Where(p => p.PostId == model.ParentId).FirstOrDefault();

                parentPost.Children++;
            }

            await forumDbContext.AddAsync(newPost);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task<List<ForumViewModel>> GetForums()
        {
            List<ForumViewModel> forumViewModels = new List<ForumViewModel>();

            var q = await forumDbContext.Forums.ToListAsync();

            foreach(var forum in q)
            {
                ForumViewModel forumViewModel = new ForumViewModel
                {
                    ForumId = forum.ForumId,
                    ForumName = forum.ForumName,
                    AuthorName = forum.AuthorName,
                    PostedOn = DateTime.Now,
                    UserId = forum.UserId
                };

                forumViewModels.Add(forumViewModel);
            }

            return forumViewModels;
        }

        //public async Task<List<Threads>> GetUserThreads(string userId)
        //{
        //    var q = await forumDbContext.Threads.Where(t => t.UserId == userId).ToListAsync();

        //    return q;
        //}

        public async Task<List<ThreadViewModel>> GetForumThreads(string forumId)
        {
            List<ThreadViewModel> threadViewModels = new List<ThreadViewModel>();

            var q = await forumDbContext.Threads
                .Where(t => t.ForumId == forumId)
                .ToListAsync();

            foreach(var thread in q)
            {
                ThreadViewModel threadViewModel = new ThreadViewModel
                {
                    ThreadId = thread.ThreadId,
                    ThreadTitle = thread.ThreadTitle,
                    ForumId = thread.ForumId,
                    Subject = thread.Subject,
                    AuthorName = thread.AuthorName,
                    UserId = thread.UserId
                };

                threadViewModels.Add(threadViewModel);
            }

            return threadViewModels;
        }

        public async Task<List<ForumViewModel>> GetForumsWithTheirThreads()
        {
            List<ForumViewModel> forumViewModels = new List<ForumViewModel>();

            forumViewModels = await GetForums();

            foreach(var forum in forumViewModels)
            {
                forum.Threads = await GetForumThreads(forum.ForumId);
            }

            return forumViewModels;
        }

        public List<TreeExtensions.ITree<Posts>> GetThreadPosts(string threadId)
        {
            List<Posts> all = forumDbContext.Posts.Include(x => x.Parent).Where(p => p.ThreadId == threadId).ToList();
            TreeExtensions.ITree<Posts> virtualRootNode = all.ToTree((parent, child) => child.ParentId == parent.PostId);
            List<TreeExtensions.ITree<Posts>> rootLevelPostsWithSubTree = virtualRootNode.Children.ToList();
            List<TreeExtensions.ITree<Posts>> flattenedListOfPostNodes = virtualRootNode.Children.Flatten(node => node.Children).ToList();

            return rootLevelPostsWithSubTree;
        }


    }
}
