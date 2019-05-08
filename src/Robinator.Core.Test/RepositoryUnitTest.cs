using System;
using System.Runtime.CompilerServices;
using Xunit;

namespace Robinator.Core.Test
{
    public abstract class RepositoryUnitTest<TRepository, TContent> where TRepository : IContentRepository<TContent> where TContent : class, IContent
    {
        protected TRepository repository;
        public abstract TContent CreateRandomContent(Guid id);
        public abstract TContent UpdateRandomContent(TContent originalContent);
        public abstract void AssertRandomContent(TContent originalContent, TContent result);
        public abstract TRepository SetupRepository([CallerMemberName]string testName = "");

        [Fact]
        public void Get()
        {
            repository = SetupRepository();
            var id = Guid.NewGuid();
            var content = CreateRandomContent(id);
            repository.CreateContent(content);
            var result = repository.GetContent(id);
            AssertRandomContent(content, result);
        }

        [Fact]
        public void GetList()
        {
            repository = SetupRepository();
            BulkCreate(2);
            var result = repository.GetList(0, 10);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetListPageLength1()
        {
            repository = SetupRepository();
            BulkCreate(2);
            var result = repository.GetList(0, 1);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void GetListPageLength3()
        {
            repository = SetupRepository();
            BulkCreate(4);
            var result = repository.GetList(0, 3);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetList2ndPage()
        {
            repository = SetupRepository();
            BulkCreate(4);
            var result = repository.GetList(1, 3);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void Update()
        {
            repository = SetupRepository();
            var id = Guid.NewGuid();
            var content = CreateRandomContent(id);
            repository.CreateContent(content);
            var updated = UpdateRandomContent(content);
            repository.UpdateContent(updated);
            var result = repository.GetContent(id);
            AssertRandomContent(updated, result);
        }

        [Fact]
        public void Delete()
        {
            repository = SetupRepository();
            var id = Guid.NewGuid();
            var content = CreateRandomContent(id);
            repository.CreateContent(content);
            repository.DeleteContent(id);
            var result = repository.GetContent(id);
            Assert.Null(result);
        }
        private void BulkCreate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                repository.CreateContent(CreateRandomContent(Guid.NewGuid()));
            }
        }
    }
}
