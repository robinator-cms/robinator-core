using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Robinator.Example.Areas.Blog.Content;
using Robinator.Example.Areas.Blog.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Robinator.Core.Areas.Blog.Pages
{
    public class GetModel : PageModel
    {
        private readonly IBlogPostRepository repository;

        public GetModel(IBlogPostRepository repository)
        {
            this.repository = repository;
        }

        public BlogPost BlogPost { get; private set; }
        [BindProperty]
        [Range(1, 5)]
        public int Stars { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                return BadRequest();
            }
            BlogPost = await repository.GetContentAsync(id.Value);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                return BadRequest();
            }
            return await OnGetAsync(id);
        }
    }
}