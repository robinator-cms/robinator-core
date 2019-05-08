using System.Threading.Tasks;
using Robinator.Core;
using Robinator.Example.Areas.News.Models;

namespace Robinator.Example.Areas.News.Content
{
    public interface INewsRepository : IContentRepository<NewsPost>
    {
        Task<NewsPost> GetContentAsync(string id);
    }
}
