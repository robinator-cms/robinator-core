using System;
using Xunit;

namespace Robinator.Core.Test
{
    public class EditorPropertyUnitTest
    {
        interface INotIEditor { }

        [Fact]
        public void NotIEditor()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new EditorPropertyAttribute(typeof(INotIEditor));
            });
        }

        [Fact]
        public void NotInterface()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new EditorPropertyAttribute(typeof(EditorPropertyUnitTest));
            });
        }

        [Fact]
        public void MustInherit()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new EditorPropertyAttribute(typeof(IEditor));
            });
        }
        [Fact]
        public void CannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new EditorPropertyAttribute(null);
            });
        }

        [Fact]
        public void WorksWithText()
        {
            new EditorPropertyAttribute(typeof(ITextEditor));
        }

        [Fact]
        public void WorksWithHtml()
        {
            new EditorPropertyAttribute(typeof(IHtmlEditor));
        }
    }
}
