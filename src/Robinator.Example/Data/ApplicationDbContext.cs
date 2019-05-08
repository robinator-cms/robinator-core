﻿using Microsoft.EntityFrameworkCore;
using Robinator.Core.EF;
using Robinator.Example.Areas.Blog.Models;
using Robinator.Example.Areas.News.Models;

namespace Robinator.Example
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostStars> BlogPostStars { get; set; }
    }
}