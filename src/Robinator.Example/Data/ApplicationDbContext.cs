using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Robinator.Core.EF;
using Robinator.Example.Areas.Blog.Models;
using Robinator.Example.Areas.News.Models;
using Robinator.Example.Components;

namespace Robinator.Example
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>, IDbContext, IRatingDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}