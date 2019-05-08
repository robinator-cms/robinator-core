using Robinator.Core.EF.Data;
using System.ComponentModel;

namespace Robinator.Core.EF.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ContentRepository<TContent> : ContentRepositoryBase<TContent, RobinatorDbContext>, IContentRepository<TContent> where TContent : class, IContent, new()
    {
        public ContentRepository(RobinatorDbContext context) : base(context)
        {
        }
    }
}
