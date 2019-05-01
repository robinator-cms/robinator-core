using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Robinator.Core.Areas.RobinatorAdmin.Pages.Content
{
    public class IndexModel : PageModel
    {
        private readonly RobinatorOptions options;

        public IndexModel(IOptionsMonitor<RobinatorOptions> options)
        {
            this.options = options.CurrentValue;
        }

        public List<DefaultEditPageConfiguation> EditTypes { get; private set; }

        public void OnGet()
        {
            EditTypes = this.options.DefaultEditPages;
        }
    }
}