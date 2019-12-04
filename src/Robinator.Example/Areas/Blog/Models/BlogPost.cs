using Microsoft.AspNetCore.Identity;
using Robinator.Core;
using Robinator.FileService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Robinator.Example.Areas.Blog.Models
{
    public class BlogPost : IContent
    {
        public Guid Id { get; set; }
        [HtmlEditor]
        [Display(Name = "Html")]
        public string Text { get; set; }
        [TextEditor]
        public string Title { get; set; }
        [FileEditor]
        public string Image { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
        public ICollection<BlogPostStars> Stars { get; private set; } = new HashSet<BlogPostStars>();
        public string CreatedById { get; set; }
        public IdentityUser CreatedBy { get; set; }
    }
}
