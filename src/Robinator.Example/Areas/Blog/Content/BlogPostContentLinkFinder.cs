using Robinator.Core;
using Robinator.Example.Areas.Blog.Models;

namespace Robinator.Example.Areas.Blog.Content
{
    public class BlogPostContentLinkFinder : ContentLinkFinder<BlogPost>
    {
        public override string LinkLookup(BlogPost content)
        {
            return $"/Blog/Get/{content.Id}";
        }
    }
}
