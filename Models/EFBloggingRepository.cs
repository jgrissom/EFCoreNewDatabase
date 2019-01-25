﻿using System;
using System.Linq;

namespace EFCoreNewDatabase.Models
{
    public class EFBloggingRepository : IBloggingRepository
    {
        // the repository class depends on the BloggingContext service
        // which was registered at application startup
        private BloggingContext context;
        public EFBloggingRepository(BloggingContext ctx)
        {
            context = ctx;
        }
        // create IQueryable for Blogs & Posts
        public IQueryable<Blog> Blogs => context.Blogs;
        public IQueryable<Post> Posts => context.Posts;

    }
}
