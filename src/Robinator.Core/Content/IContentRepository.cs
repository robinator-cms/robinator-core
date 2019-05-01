using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core
{
    public interface IContentRepository
    {
        IList<object> GetList(int page, int pageSize);
        Task<IList<object>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        object GetContent(Guid id);
        Task<object> GetContentAsync(Guid id, CancellationToken cancellationToken = default);
        void CreateContent(object content);
        Task CreateContentAsync(object content, CancellationToken cancellationToken = default);
        void UpdateContent(object content);
        Task UpdateContentAsync(object content, CancellationToken cancellationToken = default);
        void DeleteContent(Guid id);
        Task DeleteContentAsync(Guid id, CancellationToken cancellationToken = default);
        long Count();
        Task<long> CountAsync(CancellationToken cancellationToken = default);
    }
    public interface IContentRepository<TContent> : IContentRepository where TContent : class, IContent
    {
        new IList<TContent> GetList(int page, int pageSize);
        new Task<IList<TContent>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        new TContent GetContent(Guid id);
        new Task<TContent> GetContentAsync(Guid id, CancellationToken cancellationToken = default);
        void CreateContent(TContent content);
        Task CreateContentAsync(TContent content, CancellationToken cancellationToken = default);
        void UpdateContent(TContent content);
        Task UpdateContentAsync(TContent content, CancellationToken cancellationToken = default);
    }
}
