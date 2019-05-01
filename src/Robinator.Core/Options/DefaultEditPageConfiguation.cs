using Robinator.Core.Areas.RobinatorAdmin.Pages.ViewModels;
using System;

namespace Robinator.Core
{
    public class DefaultEditPageConfiguation
    {
        public DefaultEditPageConfiguation(string link, string name, Func<object, ContentHeaderViewModel> projection, Type type)
        {
            Link = link;
            Name = name;
            Projection = projection;
            Type = type;
        }
        public string Link { get; }
        public string Name { get; }
        public Func<object, ContentHeaderViewModel> Projection { get; }
        public Type Type { get; }
    }

    public class DefaultEditPageConfiguation<TContent> : DefaultEditPageConfiguation where TContent : class, IContent
    {
        public DefaultEditPageConfiguation(string link, string name, Func<object, ContentHeaderViewModel> projection) : base(link, name, projection, typeof(TContent))
        {
        }

        public DefaultEditPageConfiguation(Func<object, ContentHeaderViewModel> projection) : base(
            typeof(TContent).Name.ToLower(), typeof(TContent).Name, projection, typeof(TContent))
        {
        }
    }
}