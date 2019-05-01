using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Robinator.Core.Mongo
{
    public abstract class ContentRepository<TContent> : IContentRepository<TContent> where TContent : class, IContent, new()
    {
        private readonly IMongoCollection<TContent> collection;

        public ContentRepository(IMongoDatabase context, string collectionName)
        {
            collection = context.GetCollection<TContent>(collectionName);
        }

        public long Count() => collection.CountDocuments(Builders<TContent>.Filter.Empty);

        public Task<long> CountAsync(CancellationToken cancellationToken = default) => collection.CountDocumentsAsync(Builders<TContent>.Filter.Empty, cancellationToken: cancellationToken);

        public virtual void CreateContent(TContent content) => collection.InsertOne(content);

        public void CreateContent(object content) => CreateContent(content as TContent);

        public virtual Task CreateContentAsync(TContent content, CancellationToken cancellationToken = default) => collection.InsertOneAsync(content, cancellationToken: cancellationToken);

        public Task CreateContentAsync(object content, CancellationToken cancellationToken = default) => CreateContentAsync(content as TContent, cancellationToken);

        public virtual void DeleteContent(Guid id) => collection.DeleteOne(Builders<TContent>.Filter.Eq(x => x.Id, id));

        public virtual Task DeleteContentAsync(Guid id, CancellationToken cancellationToken = default) => collection.DeleteOneAsync(Builders<TContent>.Filter.Eq(x => x.Id, id), cancellationToken: cancellationToken);

        public virtual TContent GetContent(Guid id) => collection.Find(Builders<TContent>.Filter.Eq(x => x.Id, id)).FirstOrDefault();

        public virtual Task<TContent> GetContentAsync(Guid id, CancellationToken cancellationToken = default) => collection.Find(Builders<TContent>.Filter.Eq(x => x.Id, id)).FirstOrDefaultAsync(cancellationToken);

        public virtual IList<TContent> GetList(int page, int pageSize) => collection.Find(Builders<TContent>.Filter.Empty).ToList();

        public virtual async Task<IList<TContent>> GetListAsync(int page, int pageSize, CancellationToken cancellationToken = default) => await collection.Find(Builders<TContent>.Filter.Empty).ToListAsync(cancellationToken);

        public virtual void UpdateContent(TContent content) => collection.ReplaceOne(Builders<TContent>.Filter.Eq(x => x.Id, content.Id), content);

        public void UpdateContent(object content) => UpdateContent(content as TContent);

        public virtual Task UpdateContentAsync(TContent content, CancellationToken cancellationToken = default) => collection.ReplaceOneAsync(Builders<TContent>.Filter.Eq(x => x.Id, content.Id), content, cancellationToken: cancellationToken);

        public Task UpdateContentAsync(object content, CancellationToken cancellationToken = default) => UpdateContentAsync(content as TContent, cancellationToken);

        object IContentRepository.GetContent(Guid id) => GetContent(id);

        async Task<object> IContentRepository.GetContentAsync(Guid id, CancellationToken cancellationToken) => await GetContentAsync(id, cancellationToken);

        IList<object> IContentRepository.GetList(int page, int pageSize) => GetList(page, pageSize).Cast<object>().ToList();

        async Task<IList<object>> IContentRepository.GetListAsync(int page, int pageSize, CancellationToken cancellationToken) => (await GetListAsync(page, pageSize, cancellationToken)).Cast<object>().ToList();
    }
}
