using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Robinator.Example.Areas.News.Content;
using Robinator.Example.Areas.News.Models;
using System;
using System.Threading.Tasks;

namespace Robinator.Core.Areas.News.Pages
{
    public class GetModel : PageModel
    {
        private readonly INewsRepository repository;

        public GetModel(INewsRepository repository)
        {
            this.repository = repository;
        }

        public NewsPost News { get; private set; }

        public async Task<IActionResult> OnGet(string name)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            News = await repository.GetContentAsync(name);
            return Page();
        }
    }
}