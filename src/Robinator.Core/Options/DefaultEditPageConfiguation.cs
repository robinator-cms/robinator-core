using System;
using System.Text.RegularExpressions;

namespace Robinator.Core
{
    public class DefaultEditPageConfiguation
    {
        public DefaultEditPageConfiguation(string name, string link, Func<object, ContentHeaderViewModel> projection, Type type, string summaryPartialName)
        {
            Name = name;
            Link = link;
            Projection = projection;
            Type = type;
            SummaryPartialName = summaryPartialName;
        }
        public string Link { get; }
        public string Name { get; }
        public string SummaryPartialName { get; set; }
        public Func<object, ContentHeaderViewModel> Projection { get; }
        public Type Type { get; }
    }

    public class DefaultEditPageConfiguation<TContent> : DefaultEditPageConfiguation where TContent : class, IContent
    {
        public DefaultEditPageConfiguation(
            Func<TContent, ContentHeaderViewModel> projection,
            string name = null,
            string link = null,
            string summaryPartialName = null
        ) : base(
            name ?? Regex.Replace(typeof(TContent).Name, "[A-Z]", " $0"),
            link ?? typeof(TContent).Name.ToLower(),
            o => projection(o as TContent),
            typeof(TContent),
            summaryPartialName
        )
        {
        }
    }
}