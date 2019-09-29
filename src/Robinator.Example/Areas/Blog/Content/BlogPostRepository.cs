using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Robinator.Core.EF;
using Robinator.Example.Areas.Blog.Models;

namespace Robinator.Example.Areas.Blog.Content
{
    public class BlogPostRepository : ContentRepositoryBase<BlogPost, ApplicationDbContext>, IBlogPostRepository
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BlogPostRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public override void CreateContent(BlogPost content)
        {
            content.CreatedById = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            content.PublishedAt = DateTimeOffset.Now;
            base.CreateContent(content);
        }
        public override Task CreateContentAsync(BlogPost content, CancellationToken cancellationToken = default)
        {
            content.CreatedById = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            content.PublishedAt = DateTimeOffset.Now;
            return base.CreateContentAsync(content, cancellationToken);
        }
        public override IQueryable<BlogPost> Query()
        {
            return base.Query().Include(x => x.Stars).Include(x => x.CreatedBy);
        }
        public async Task RateAsync(Guid id, int stars)
        {
            var user = httpContextAccessor.HttpContext.User;
            if (stars < 1 || stars > 5 || !signInManager.IsSignedIn(user) || !Query().Any(x => x.Id == id))
            {
                throw new ArgumentOutOfRangeException();
            }
            var userId = userManager.GetUserId(user);
            var oldRating = await context.BlogPostStars.SingleOrDefaultAsync(x => x.UserId == userId);
            if (oldRating == null) {
                context.BlogPostStars.Add(new BlogPostStars
                {
                    BlogPostId = id,
                    Stars = stars,
                    UserId = userId,
                });
            }
            else
            {
                oldRating.Stars = stars;
            }
            await context.SaveChangesAsync();
        }
    }
}
