using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Robinator.Core.Editors;

namespace Robinator.FileService.UI.Default
{
    public class FileEditor : IFileEditor
    {
        public object GetValue(string name, HttpRequest request)
        {
            var a = request.Form[name];
            return a.ToString();
        }

        public EditorTagData GetView(string name, object data)
        {
            var divBuilder = new TagBuilder("div");
            var buttonBuilder = new TagBuilder("button");
            buttonBuilder.Attributes.Add("type", "button");
            buttonBuilder.Attributes.Add("id", name);
            buttonBuilder.AddCssClass("filePicker");
            buttonBuilder.InnerHtml.Append("Select file");
            divBuilder.InnerHtml.AppendHtml(buttonBuilder.RenderStartTag());
            divBuilder.InnerHtml.AppendHtml(buttonBuilder.RenderBody());
            divBuilder.InnerHtml.AppendHtml(buttonBuilder.RenderEndTag());
            var hiddenBuilder = new TagBuilder("input");
            hiddenBuilder.Attributes.Add("name", name);
            hiddenBuilder.Attributes.Add("hidden", name);
            hiddenBuilder.Attributes.Add("value", data as string);
            hiddenBuilder.AddCssClass("filePicker");
            divBuilder.InnerHtml.AppendHtml(hiddenBuilder.RenderStartTag());
            divBuilder.InnerHtml.AppendHtml(hiddenBuilder.RenderBody());
            divBuilder.InnerHtml.AppendHtml(hiddenBuilder.RenderEndTag());
            var script = new TagBuilder("script");
            script.InnerHtml.AppendHtml($"document.getElementById('{name}').addEventListener('click', function(ev) {{window.fileSelected = function(file) {{document.getElementsByName('{name}')[0].setAttribute('value',file);}};" +
                "window.open('/robinatoradmin/fileuploader', '_blank', 'toolbar=yes,scrollbars=yes,resizable=yes,top=100,left=100,width=800,height=600');})");
            return new EditorTagData(divBuilder, script); 
        }
    }
}
