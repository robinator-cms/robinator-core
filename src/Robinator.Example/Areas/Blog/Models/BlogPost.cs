using Microsoft.AspNetCore.Identity;
using Robinator.Core;
using Robinator.Example.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Robinator.Example.Areas.Blog.Models
{
    public class BlogPost : IContent, IRateable
    {
        public Guid Id { get; set; }
        [HtmlEditor]
        [Display(Name = "Html")]
        public string Text { get; set; }
        [TextEditor]
        public string Title { get; set; }
        [TextEditor]
        public string Image { get; set; }
        public DateTimeOffset PublishedAt { get; set; }
        public ICollection<Rating> Stars { get; private set; } = new HashSet<Rating>();
        public string CreatedById { get; set; }
        public IdentityUser CreatedBy { get; set; }
    }
}
