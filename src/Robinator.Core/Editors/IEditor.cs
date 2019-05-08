using Microsoft.AspNetCore.Http;
using Robinator.Core.Editors;

namespace Robinator.Core
{
    public interface IEditor
    {
        EditorTagData GetView(string name, object data);
        object GetValue(string name, HttpRequest request);
    }
}
