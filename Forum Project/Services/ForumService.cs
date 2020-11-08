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

        public async Task EditPost(Posts post)
        {
            forumDbContext.Update(post);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task EditForum(Forums forum)
        {
            forumDbContext.Forums.Update(forum);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task EditThread(Threads thread)
        {
            forumDbContext.Threads.Update(thread);
            await forumDbContext.SaveChangesAsync();
        }

        public async Task<List<ForumViewModel>> GetForums()
        {
            List<ForumViewModel> forumViewModels = new List<ForumViewModel>();

            var q = await forumDbContext.Forums.ToListAsync();

            foreach (var forum in q)
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

        public async Task DeleteForum(string forumId)
        {
            var posts = forumDbContext.Posts.Where(p => p.ForumId == forumId);
            forumDbContext.RemoveRange(posts);

            var threads = forumDbContext.Threads.Where(t => t.ForumId == forumId);
            forumDbContext.RemoveRange(threads);

            var forum = forumDbContext.Forums.Where(f => f.ForumId == forumId);
            forumDbContext.RemoveRange(forum);

            await forumDbContext.SaveChangesAsync();
        }

        public async Task DeleteThread(string threadId)
        {
            var posts = forumDbContext.Posts.Where(p => p.ThreadId == threadId);
            forumDbContext.RemoveRange(posts);

            var threads = forumDbContext.Threads.Where(t => t.ThreadId == threadId);
            forumDbContext.RemoveRange(threads);

            await forumDbContext.SaveChangesAsync();
        }

        public Forums GetForumModel(string forumId)
        {
            var q = forumDbContext.Forums.Where(f => f.ForumId == forumId).FirstOrDefault();
            return q;
        }

        public ForumViewModel GetForum(string forumId)
        {
            var q = forumDbContext.Forums.Where(f => f.ForumId == forumId).FirstOrDefault();

            ForumViewModel forumViewModel = new ForumViewModel
            {
                ForumId = q.ForumId,
                AuthorName = q.AuthorName,
                ForumName = q.ForumName,
                PostedOn = q.PostedOn,
                UserId = q.UserId
            };

            return forumViewModel;
        }

        public ThreadViewModel GetThread(string threadID)
        {
            var q = forumDbContext.Threads.Where(t => t.ThreadId == threadID).FirstOrDefault();

            ThreadViewModel threadViewModel = new ThreadViewModel()
            {
                ThreadId = q.ThreadId,
                ForumId = q.ForumId,
                AuthorName = q.AuthorName,
                PostedOn = q.PostedOn,
                Subject = q.Subject,
                ThreadTitle = q.ThreadTitle,
                UserId = q.UserId
            };

            var forum = GetForum(threadViewModel.ForumId);

            threadViewModel.ForumName = forum.ForumName;

            return threadViewModel;
        }
        public Threads GetThreadModel(string threadId)
        {
            var q = forumDbContext.Threads.Where(f => f.ThreadId == threadId).FirstOrDefault();
            return q;
        }

        public async Task<Posts> GetPost(string postId)
        {
            var q = await forumDbContext.Posts.Where(p => p.PostId == postId).FirstOrDefaultAsync();

            return q;
        }

        public async Task<List<Threads>> GetUserThreads(string userId)
        {
            var q = await forumDbContext.Threads.Where(t => t.UserId == userId).ToListAsync();

            return q;
        }

        public async Task<List<ThreadViewModel>> GetForumThreads(string forumId)
        {
            List<ThreadViewModel> threadViewModels = new List<ThreadViewModel>();

            var q = await forumDbContext.Threads
                .Where(t => t.ForumId == forumId)
                .ToListAsync();

            foreach (var thread in q)
            {
                ThreadViewModel threadViewModel = new ThreadViewModel
                {
                    ThreadId = thread.ThreadId,
                    ThreadTitle = thread.ThreadTitle,
                    ForumId = thread.ForumId,
                    PostedOn = thread.PostedOn,
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

            foreach (var forum in forumViewModels)
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
            //List<TreeExtensions.ITree<Posts>> flattenedListOfPostNodes = virtualRootNode.Children.Flatten(node => node.Children).ToList();

            return rootLevelPostsWithSubTree;
        }

        public async Task DeletePost(Posts post)
        {
            var target = forumDbContext.Posts
            .Include(x => x.ChildrenPosts)
            .FirstOrDefault(x => x.ParentId == post.PostId);

            if (target != null)
            {
                RecursiveDelete(target);
            }

            forumDbContext.Remove(post);
            await forumDbContext.SaveChangesAsync();
        }
        private void RecursiveDelete(Posts parent)
        {
            if (parent.ChildrenPosts != null)
            {
                var children = forumDbContext.Posts
                    .Include(x => x.ChildrenPosts)
                    .Where(x => x.ParentId == parent.PostId);

                foreach (var child in children)
                {
                    RecursiveDelete(child);
                }
            }

            forumDbContext.Remove(parent);
        }

        public string RelativeDate(DateTime theDate)
        {
            Dictionary<long, string> thresholds = new Dictionary<long, string>();
            int minute = 60;
            int hour = 60 * minute;
            int day = 24 * hour;
            thresholds.Add(60, "{0} seconds ago");
            thresholds.Add(minute * 2, "a minute ago");
            thresholds.Add(45 * minute, "{0} minutes ago");
            thresholds.Add(120 * minute, "an hour ago");
            thresholds.Add(day, "{0} hours ago");
            thresholds.Add(day * 2, "yesterday");
            thresholds.Add(day * 30, "{0} days ago");
            thresholds.Add(day * 365, "{0} months ago");
            thresholds.Add(long.MaxValue, "{0} years ago");
            long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
            foreach (long threshold in thresholds.Keys)
            {
                if (since < threshold)
                {
                    TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
                    return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
                }
            }
            return "";
        }
    }
}
