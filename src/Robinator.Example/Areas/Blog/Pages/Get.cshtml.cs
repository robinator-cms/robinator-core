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

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                return BadRequest();
            }
            BlogPost = await repository.GetContentAsync(id.Value);
            return Page();
        }
        public async Task<IActionResult> OnPost(Guid? id, [Range(1, 5)]int stars)
        {
            if (!ModelState.IsValid || !id.HasValue)
            {
                return BadRequest();
            }
            await repository.RateAsync(id.Value, stars);
            return await OnGet(id);
        }
    }
}