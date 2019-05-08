using Robinator.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace Robinator.Example.Areas.News.Models
{
    public class NewsPost : IContent
    {
        public Guid Id { get; set; }
        [HtmlEditor]
        [Display(Name = "Html")]
        public string Text { get; set; }
        [TextEditor]
        public string Title { get; set; }
        [TextEditor]
        public string Image { get; set; }
        public DateTimeOffset PublishedAt { get; set; } = DateTimeOffset.Now;
    }
}
