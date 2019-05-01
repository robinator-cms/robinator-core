using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Robinator.Core.Areas.Pages
{
    public class GetModel : PageModel
    {
        private readonly IPageRepository repository;

        public GetModel(IPageRepository repository)
        {
            this.repository = repository;
        }

        public Page PageEntity { get; private set; }

        public IActionResult OnGet(Guid? id)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                return BadRequest();
            }
            PageEntity = repository.GetContent(id.Value);
            return Page();
        }
    }
}