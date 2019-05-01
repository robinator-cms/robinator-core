using Microsoft.EntityFrameworkCore;
using Robinator.Core.EF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core.EF
{
    public abstract class ContentRepository<TContent> : IContentRepository<TContent> where TContent : class, IContent, new()
    {
        private readonly RobinatorDbContext context;

        public ContentRepository(RobinatorDbContext context)
        {
            this.context = context;
        }

        public long Count() => context.Set<TContent>().LongCount();

        public Task<long> CountAsync(CancellationToken cancellationToken = default) => context.Set<TContent>().LongCountAsync(cancellationToken);

        public virtual void CreateContent(TContent content)
        {
            context.Set<TContent>().Add(content);
            context.SaveChanges();
        }

        public void CreateContent(object content) => CreateContent(content as TContent);

        public virtual async Task CreateContentAsync(TContent content, CancellationToken cancellationToken = default)
        {
            await context.Set<TContent>().AddAsync(content);
            await context.SaveChangesAsync();
        }

        public Task CreateContentAsync(object content, CancellationToken cancellationToken = default) => CreateContentAsync(content as TContent, cancellationToken);
        public virtual void DeleteContent(Guid id)
        {
            context.Set<TContent>().Remove(new TContent
            {
                Id = id
            });
            context.SaveChanges();
        }

        public virtual async Task DeleteContentAsync(Guid id, CancellationToken cancellationToken = default)
        {
            context.Set<TContent>().Remove(new TContent
            {
                Id = id
            });
            await context.SaveChangesAsync();
        }

        public virtual TContent GetContent(Guid id) => context.Set<TContent>().AsNoTracking().SingleOrDefault(c => c.Id == id);

        public virtual Task<TContent> GetContentAsync(Guid id, CancellationToken cancellationToken = default) => context.Set<TContent>().AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);

        public virtual IList<TContent> GetList(int page, int pageSize) => context.Set<TContent>().Skip(page * pageSize).Take(pageSize).AsNoTracking().ToList();

        public virtual async Task<IList<TContent>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default) => await context.Set<TContent>().Skip(page * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

        public virtual void UpdateContent(TContent content)
        {
            context.Set<TContent>().Update(content);
            context.SaveChanges();
        }

        public void UpdateContent(object content) => UpdateContent(content as TContent);

        public virtual async Task UpdateContentAsync(TContent content, CancellationToken cancellationToken = default)
        {
            context.Set<TContent>().Update(content);
            await context.SaveChangesAsync();
        }

        public Task UpdateContentAsync(object content, CancellationToken cancellationToken = default) => UpdateContentAsync(content as TContent, cancellationToken);
        object IContentRepository.GetContent(Guid id) => GetContent(id);

        async Task<object> IContentRepository.GetContentAsync(Guid id, CancellationToken cancellationToken) => await GetContentAsync(id, cancellationToken);

        IList<object> IContentRepository.GetList(int page, int pageSize) => GetList(page, pageSize).Cast<object>().ToList();

        async Task<IList<object>> IContentRepository.GetListAsync(int page, int pageSize, CancellationToken cancellationToken) => (await GetListAsync(page, pageSize, cancellationToken)).Cast<object>().ToList();
    }
}
