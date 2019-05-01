using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core.Areas.Pages
{
    public class PageContentLinkFinder : ContentLinkFinder<Page>
    {
        public override string LinkLookup(Page content)
        {
            return $"/Pages/Get/{content.Id}";
        }

        public override Task<string> LinkLookupAsync(Page content, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(LinkLookup(content));
        }
    }
}
