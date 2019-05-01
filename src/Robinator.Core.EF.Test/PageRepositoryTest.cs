using System;
using Microsoft.EntityFrameworkCore;
using Robinator.Core.Areas.Pages;
using Robinator.Core.EF.Data;
using Robinator.Core.EF.Pages;
using Robinator.Core.Test;
using Xunit;

namespace Robinator.Core.EF.Test
{
    public class PageRepositoryTest : RepositoryUnitTest<PageRepository, Page>
    {
        public PageRepositoryTest() : base(new PageRepository(new RobinatorDbContext(new DbContextOptionsBuilder<RobinatorDbContext>().UseInMemoryDatabase("test").Options)))
        {
        }

        public override void AssertRandomContent(Page originalContent, Page result)
        {
            Assert.Equal(originalContent.Id, result.Id);
            Assert.Equal(originalContent.Text, result.Text);
            Assert.Equal(originalContent.Title, result.Title);
        }

        public override Page CreateRandomContent(Guid id) => new Page
        {
            Id = id,
            Text = "Test text " + id.ToString().Substring(0, 4),
            Title = "Test title" + id.ToString().Substring(4, 4),
        };

        public override Page UpdateRandomContent(Page originalContent) => new Page
        {
            Id = originalContent.Id,
            Text = "Test text " + originalContent.Id.ToString().Substring(4, 4),
            Title = "Test title" + originalContent.Id.ToString().Substring(0, 4),
        };
    }
}
