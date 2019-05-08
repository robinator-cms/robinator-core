using Microsoft.EntityFrameworkCore;
using Robinator.Core.EF;
using Robinator.Example.Areas.News.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Robinator.Example.Areas.News.Content
{
    public class NewsRepository : ContentRepositoryBase<NewsPost, ApplicationDbContext>, INewsRepository
    {
        public NewsRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<NewsPost> GetContentAsync(string title) => context.NewsPosts.Where(x => x.Title == title).SingleOrDefaultAsync();
    }
}
