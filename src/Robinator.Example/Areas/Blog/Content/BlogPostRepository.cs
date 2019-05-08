using Robinator.Core.EF;
using Robinator.Example.Areas.Blog.Models;

namespace Robinator.Example.Areas.Blog.Content
{
    public class BlogPostRepository : ContentRepositoryBase<BlogPost, ApplicationDbContext>, IBlogPostRepository
    {
        public BlogPostRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
