using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Robinator.Core.Editors.Defaults
{
    internal class HtmlEditor : IHtmlEditor
    {
        public object GetValue(string name, HttpRequest request)
        {
            var a = request.Form[name];
            return a.ToString();
        }

        public TagBuilder GetView(string name, object data)
        {
            var builder = new TagBuilder("textarea");
            builder.Attributes.Add("name", name);
            builder.InnerHtml.AppendHtml(data as string);
            return builder;
        }
    }
}
