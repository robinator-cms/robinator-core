using Robinator.FileService;
using Robinator.FileService.LocalProvider;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRobinator(this IServiceCollection services, Action<LocalFileLocatorOptions> options)
        {
            services.Configure(options);
            services.AddTransient<IFileLocatorService, LocalFileLocatorService>();
            return services;
        }
    }
}
