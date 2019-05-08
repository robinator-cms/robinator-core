using Robinator.Core.Areas.Pages;
using Robinator.Core.EF.Data;
using Robinator.Core.EF.Internal;
using System.ComponentModel;

namespace Robinator.Core.EF.Pages
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PageRepository : ContentRepository<Page>, IPageRepository
    {
        public PageRepository(RobinatorDbContext context) : base(context)
        {
        }
    }
}
