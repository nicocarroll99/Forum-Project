using Forum_Project.Context;
using Forum_Project.Migrations;
using Forum_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddForum(Forums model)
        {
            var newForum = new Forums
            {
                ForumName = model.ForumName
            };

            forumDbContext.Forums.Add(newForum);
            forumDbContext.SaveChanges();
        }


    }
}
