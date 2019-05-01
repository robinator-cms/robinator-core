using Robinator.Core.Areas.Pages;
using Robinator.Core.EF.Data;

namespace Robinator.Core.EF.Pages
{
    public class PageRepository : ContentRepository<Page>, IPageRepository
    {
        public PageRepository(RobinatorDbContext context) : base(context)
        {
        }
    }
}
