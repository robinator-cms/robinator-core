using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core
{
    public abstract class ContentLinkFinder<TContent> : IContentLinkFinder<TContent> where TContent : class, IContent
    {
        public abstract string LinkLookup(TContent content);
        public string LinkLookup(object content) => LinkLookup(content as TContent);
        public virtual Task<string> LinkLookupAsync(TContent content, CancellationToken cancellationToken = default) => Task.FromResult(LinkLookup(content));
        public Task<string> LinkLookupAsync(object content, CancellationToken cancellationToken = default) => LinkLookupAsync(content as TContent, cancellationToken);
    }
}
