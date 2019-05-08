using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Robinator.Core;
using Robinator.Core.Editors;
using System;

namespace Robinator.Editors.CKEditor
{
    public class CKEditorHtmlEditor : IHtmlEditor
    {
        public object GetValue(string name, HttpRequest request)
        {
            var a = request.Form[name];
            return a.ToString();
        }

        public EditorTagData GetView(string name, object data)
        {
            var builder = new TagBuilder("textarea");
            builder.Attributes.Add("name", name);
            builder.Attributes.Add("id", $"CKE-{name}");
            builder.InnerHtml.AppendHtml(data as string);
            var script = new TagBuilder("script");
            script.InnerHtml.AppendHtml($"CKEDITOR.replace('CKE-{name}');");
            return new EditorTagData(builder, script);
        }
    }
}
