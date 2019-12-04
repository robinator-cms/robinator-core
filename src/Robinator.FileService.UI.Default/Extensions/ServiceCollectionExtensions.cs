using Robinator.FileService;
using Robinator.FileService.UI.Default;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRobinatorFileServiceDefaultUI(this IServiceCollection services)
        {
            foreach (var editor in services.Where(x => x.ServiceType == typeof(IFileEditor)).ToList())
            {
                services.Remove(editor);
            }
            services.AddSingleton<IFileEditor, FileEditor>();
            return services;
        }
    }
}
