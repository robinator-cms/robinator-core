using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Robinator.Core.Areas.Content.Pages.ViewModels;

namespace Robinator.Core.Areas.Content.Pages
{
    public class ListModel : PageModel
    {
        private readonly RobinatorOptions options;
        private readonly IServiceProvider serviceProvider;

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 0;
        public long Count { get; set; }
        public List<ContentHeaderViewModel> Contents { get; } = new List<ContentHeaderViewModel>();
        public string TypeName { get; private set; }

        public ListModel(IOptionsMonitor<RobinatorOptions> options, IServiceProvider serviceProvider)
        {
            this.options = options.CurrentValue;
            this.serviceProvider = serviceProvider;
        }
        public async Task OnGet(CancellationToken cancellationToken)
        {
            var editorDefinition = options.DefaultEditPages.Find(x => x.Link == Type);
            if (editorDefinition == null)
            {
                throw new ArgumentOutOfRangeException($"No type is found with default edit link {Type}");
            }
            var dataType = editorDefinition.Type;
            var repository = CreateRepository(dataType);
            var linkFinder = CreateLinkFinder(dataType);
            Count = await repository.CountAsync(cancellationToken);
            if (PageNumber < 0 || Count == 0 && PageNumber != 0)
            {
                PageNumber = 0;
            }
            if (PageNumber >= (Count - 1) / 10)
            {
                PageNumber = (int)((Count - 1) / 10);
            }
            var list = repository.GetList(PageNumber, 10);
            foreach (var item in list as IEnumerable)
            {
                var x = editorDefinition.Projection(item);
                Contents.Add(new ContentHeaderViewModel
                {
                    Text = x.Text,
                    Link = await linkFinder.LinkLookupAsync(item),
                });
            }
            TypeName = editorDefinition.Name;
        }
        private IContentRepository CreateRepository(Type dataType)
        {
            return serviceProvider.GetService(typeof(IContentRepository<>).MakeGenericType(dataType)) as IContentRepository;
        }
        private IContentLinkFinder CreateLinkFinder(Type dataType)
        {
            return serviceProvider.GetService(typeof(IContentLinkFinder<>).MakeGenericType(dataType)) as IContentLinkFinder;
        }
    }
}