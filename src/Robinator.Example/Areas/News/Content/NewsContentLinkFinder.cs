using Robinator.Core;
using Robinator.Example.Areas.News.Models;

namespace Robinator.Example.Areas.News.Content
{
    public class NewsContentLinkFinder : ContentLinkFinder<NewsPost>
    {
        public override string LinkLookup(NewsPost content)
        {
            return $"/News/Get/{content.Title}";
        }
    }
}
