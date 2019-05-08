using Robinator.Core;
using Robinator.Editors.CKEditor;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRobinatorCKEditor(this IServiceCollection services)
        {
            foreach (var htmlEditor in services.Where(x => x.ServiceType == typeof(IHtmlEditor)).ToList())
            {
                services.Remove(htmlEditor);
            }
            services.AddSingleton<IHtmlEditor, CKEditorHtmlEditor>();
            return services;
        }
    }
}
