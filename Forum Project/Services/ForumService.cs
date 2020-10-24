using Forum_Project.Context;
using Forum_Project.Migrations;
using Forum_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task AddForum(Forums model)
        {
            var newForum = new Forums
            {
                ForumName = model.ForumName
            };

            forumDbContext.Forums.Add(newForum);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task AddThread(int forumId, Threads model)
        {
            //var newThread = new Threads
            //{
            //    ForumIdFK = forumId,
            //    Subject = model.Subject,
            //    UserIdFK = model.UserIdFK
            //};

            //await forumDbContext.Threads.AddAsync(newThread);
            //await forumDbContext.SaveChangesAsync();
        }

        public async Task AddPost(int threadId, Posts model)
        {
            //var newPost = new Posts
            //{
            //    ThreadIdFK = threadId,
            //    Message = model.Message,
            //    ParentIdFK = model.ParentIdFK,
            //    PostedOn = DateTime.Now,
            //    UserIdFK = model.UserIdFK,
            //    Children = 0
            //};

            //if (model.ParentIdFK != 0)
            //{
            //    var parentPost = forumDbContext.Posts
            //        .Where(p => p.PostId == model.ParentIdFK).FirstOrDefault();

            //    parentPost.Children++;
            //}

            //await forumDbContext.AddAsync(newPost);
            //await forumDbContext.SaveChangesAsync();
        }

        public async Task<List<Forums>> GetForums()
        {
            return await forumDbContext.Forums.ToListAsync();
        }

        //public async Task<List<Threads>> GetForumThreads(int forumId)
        //{
        //    var q = await forumDbContext.Threads
        //        .Where(t => t.ForumIdFK == forumId)
        //        .ToListAsync();

        //    return q;
        //}


    }
}
