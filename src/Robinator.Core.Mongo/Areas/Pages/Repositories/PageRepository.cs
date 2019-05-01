using MongoDB.Driver;
using Robinator.Core.Areas.Pages;

namespace Robinator.Core.Mongo.Pages
{
    public class PageRepository : ContentRepository<Page>, IPageRepository
    {
        public PageRepository(IMongoDatabase context) : base(context, "pages")
        {
        }
    }
}
