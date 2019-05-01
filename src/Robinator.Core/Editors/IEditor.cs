using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Robinator.Core
{
    public interface IEditor
    {
        TagBuilder GetView(string name, object data);
        object GetValue(string name, HttpRequest request); 
    }
}
