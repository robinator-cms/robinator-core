using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core
{
    public interface IContentLinkFinder
    {
        string LinkLookup(object content);
        Task<string> LinkLookupAsync(object content, CancellationToken cancellationToken = default);
    }
    public interface IContentLinkFinder<TContent> : IContentLinkFinder where TContent : class, IContent
    {
        string LinkLookup(TContent content);
        Task<string> LinkLookupAsync(TContent content, CancellationToken cancellationToken = default);
    }
}
