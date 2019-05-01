using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Robinator.Core.Areas.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPageRepository repository;

        public IndexModel(IPageRepository repository)
        {
            this.repository = repository;
        }

        public IList<Page> PageEntities { get; private set; }

        public IActionResult OnGet()
        {
            PageEntities = repository.GetList(0, 10);
            return Page();
        }
    }
}