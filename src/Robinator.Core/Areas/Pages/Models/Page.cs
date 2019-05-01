using System;
using System.ComponentModel.DataAnnotations;

namespace Robinator.Core.Areas.Pages
{
    public class Page : IContent
    {
        [HtmlEditor]
        [Display(Name = "Html")]
        public string Text { get; set; }
        [TextEditor]
        public string Title { get; set; }
        public Guid Id { get; set; }
    }
}
