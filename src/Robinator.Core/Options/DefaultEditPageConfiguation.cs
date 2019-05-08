using System;
using System.Text.RegularExpressions;

namespace Robinator.Core
{
    public class DefaultEditPageConfiguation
    {
        public DefaultEditPageConfiguation(string name, string link, Func<object, ContentHeaderViewModel> projection, Type type)
        {
            Name = name;
            Link = link;
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
        public DefaultEditPageConfiguation(string name, string link, Func<object, ContentHeaderViewModel> projection) : base(link, name, projection, typeof(TContent))
        {
        }
        public DefaultEditPageConfiguation(string name, Func<TContent, ContentHeaderViewModel> projection) : base(
            name, typeof(TContent).Name.ToLower(), o => projection(o as TContent), typeof(TContent))
        {
        }

        public DefaultEditPageConfiguation(Func<TContent, ContentHeaderViewModel> projection) : base(
            Regex.Replace(typeof(TContent).Name, "[A-Z]", " $0"), typeof(TContent).Name.ToLower(), o => projection(o as TContent), typeof(TContent))
        {
        }
    }
}