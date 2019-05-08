using Microsoft.AspNetCore.Identity;
using System;

namespace Robinator.Example.Areas.Blog.Models
{
    public class BlogPostStars
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int Stars { get; set; }
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
    }
}
