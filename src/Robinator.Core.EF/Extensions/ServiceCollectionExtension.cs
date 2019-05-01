using Microsoft.EntityFrameworkCore;
using Robinator.Core;
using Robinator.Core.Areas.Pages;
using Robinator.Core.EF.Data;
using Robinator.Core.EF.Pages;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRobinatorDefaultEntityFramework(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
        {
            services.AddDbContext<RobinatorDbContext>(optionsAction, contextLifetime, optionsLifetime);
            services.AddTransient<IPageRepository, PageRepository>();
            services.AddTransient<IContentRepository<Page>, PageRepository>();
            return services;
        }
    }
}
