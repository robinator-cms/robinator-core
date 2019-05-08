using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core.EF
{
    public abstract class ContentRepositoryBase<TContent, TDbContext> : IContentRepository<TContent> where TContent : class, IContent, new() where TDbContext : IDbContext
    {
        protected readonly TDbContext context;

        public ContentRepositoryBase(TDbContext context)
        {
            this.context = context;
        }

        public long Count() => context.Set<TContent>().LongCount();

        public Task<long> CountAsync(CancellationToken cancellationToken = default) => context.Set<TContent>().LongCountAsync(cancellationToken);

        public virtual void CreateContent(TContent content)
        {
            context.Set<TContent>().Add(content);
            context.SaveChanges();
            context.Entry(content).State = EntityState.Detached;
        }

        public void CreateContent(object content) => CreateContent(content as TContent);

        public virtual async Task CreateContentAsync(TContent content, CancellationToken cancellationToken = default)
        {
            await context.Set<TContent>().AddAsync(content, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            context.Entry(content).State = EntityState.Detached;
        }

        public Task CreateContentAsync(object content, CancellationToken cancellationToken = default) => CreateContentAsync(content as TContent, cancellationToken);
        public virtual void DeleteContent(Guid id)
        {
            var content = new TContent
            {
                Id = id
            };
            context.Set<TContent>().Remove(content);
            context.SaveChanges();
            context.Entry(content).State = EntityState.Detached;
        }

        public virtual async Task DeleteContentAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var content = new TContent
            {
                Id = id
            };
            context.Set<TContent>().Remove(content);
            await context.SaveChangesAsync(cancellationToken);
            context.Entry(content).State = EntityState.Detached;
        }

        public virtual TContent GetContent(Guid id) => Query().SingleOrDefault(c => c.Id == id);

        public virtual Task<TContent> GetContentAsync(Guid id, CancellationToken cancellationToken = default) => Query().SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        public virtual IList<TContent> GetList(int page, int pageSize) => Query().Skip(page * pageSize).Take(pageSize).ToList();

        public virtual async Task<IList<TContent>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default) => await Query().Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        public virtual void UpdateContent(TContent content)
        {
            context.Set<TContent>().Update(content);
            context.SaveChanges();
            context.Entry(content).State = EntityState.Detached;
        }

        public void UpdateContent(object content) => UpdateContent(content as TContent);

        public virtual async Task UpdateContentAsync(TContent content, CancellationToken cancellationToken = default)
        {
            context.Set<TContent>().Update(content);
            await context.SaveChangesAsync(cancellationToken);
            context.Entry(content).State = EntityState.Detached;
        }

        public Task UpdateContentAsync(object content, CancellationToken cancellationToken = default) => UpdateContentAsync(content as TContent, cancellationToken);
        object IContentRepository.GetContent(Guid id) => GetContent(id);

        async Task<object> IContentRepository.GetContentAsync(Guid id, CancellationToken cancellationToken) => await GetContentAsync(id, cancellationToken);

        IList<object> IContentRepository.GetList(int page, int pageSize) => GetList(page, pageSize).Cast<object>().ToList();

        async Task<IList<object>> IContentRepository.GetListAsync(int page, int pageSize, CancellationToken cancellationToken) => (await GetListAsync(page, pageSize, cancellationToken)).Cast<object>().ToList();

        public virtual IQueryable<TContent> Query()
        {
            return context.Set<TContent>().AsNoTracking();
        }
    }
}
