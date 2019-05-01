using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Robinator.Core.Editors.Defaults
{
    internal class TextEditor : ITextEditor
    {
        public object GetValue(string name, HttpRequest request)
        {
            var a = request.Form[name];
            return a.ToString();
        }

        public TagBuilder GetView(string name, object data)
        {
            var builder = new TagBuilder("input");
            builder.Attributes.Add("name", name);
            builder.Attributes.Add("value", data?.ToString());
            builder.Attributes.Add("type", "text");
            builder.TagRenderMode = TagRenderMode.SelfClosing;
            return builder;
        }
    }
}
