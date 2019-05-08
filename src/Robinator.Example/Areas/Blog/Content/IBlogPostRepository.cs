using System;
using System.Threading.Tasks;
using Robinator.Core;
using Robinator.Example.Areas.Blog.Models;

namespace Robinator.Example.Areas.Blog.Content
{
    public interface IBlogPostRepository : IContentRepository<BlogPost>
    {
        Task RateAsync(Guid value, int stars);
    }
}
