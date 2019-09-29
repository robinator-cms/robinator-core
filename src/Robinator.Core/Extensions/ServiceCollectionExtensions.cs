using System;
using System.Linq;
using System.Reflection;
using Robinator.Core;
using Robinator.Core.Areas.Pages;
using Robinator.Core.Editors.Defaults;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRobinator(this IServiceCollection services, Action<RobinatorOptions> options)
        {
            services.Configure(options);
            services.AddSingleton<ITextEditor, TextEditor>();
            services.AddSingleton<IHtmlEditor, HtmlEditor>();
            services.AddSingleton<IContentLinkFinder<Page>, PageContentLinkFinder>();
            return services;
        }
        public static IServiceCollection AddRobinatorDeafult(this IServiceCollection services)
        {
            return services.AddRobinator(options =>
            {
                options.DefaultEditPages.Add(new DefaultEditPageConfiguation<Page>(x => new ContentHeaderViewModel
                {
                    Id = x.Id,
                    Text = x.Title,
                }));
            });
        }

        public static IServiceCollection AddRobinatorType<TEntity, TRepository>(this IServiceCollection services, Action<RobinatorOptions> options = null, DefaultEditPageConfiguation<TEntity> editPageConfiguration = null)
            where TEntity : class, IContent
            where TRepository : class, IContentRepository<TEntity>
        {
            services.AddTransient<IContentRepository<TEntity>, TRepository>();
            if (options != null)
            {
                services.PostConfigure(options);
            }
            if (editPageConfiguration != null)
            {
                services.PostConfigure<RobinatorOptions>(o => o.DefaultEditPages.Add(editPageConfiguration));
            }
            return services;
        }

        public static IServiceCollection AddRobinatorType<TEntity, TRepository, TRepositoryInterface>(this IServiceCollection services, Action<RobinatorOptions> options = null, DefaultEditPageConfiguation<TEntity> editPageConfiguration = null)
            where TEntity : class, IContent
            where TRepository : class, TRepositoryInterface
            where TRepositoryInterface : class, IContentRepository<TEntity>
        {
            services.AddTransient<TRepositoryInterface, TRepository>();
            return services.AddRobinatorType<TEntity, TRepository>(options, editPageConfiguration);
        }

        public static IServiceCollection AddRobinatorType<TEntity, TRepository, TRepositoryInterface, TContentLinkFinder>(this IServiceCollection services, Action<RobinatorOptions> options = null, DefaultEditPageConfiguation<TEntity> editPageConfiguration = null)
            where TEntity : class, IContent
            where TRepository : class, TRepositoryInterface
            where TRepositoryInterface : class, IContentRepository<TEntity>
            where TContentLinkFinder : class, IContentLinkFinder<TEntity>
        {
            services.AddSingleton<IContentLinkFinder<TEntity>, TContentLinkFinder>();
            return services.AddRobinatorType<TEntity, TRepository, TRepositoryInterface>(options, editPageConfiguration);
        }

        public static IServiceCollection AddRobinatorTypeDynamic<TEntity>(this IServiceCollection services, Assembly assembly = null, Action<RobinatorOptions> options = null, DefaultEditPageConfiguation<TEntity> editPageConfiguration = null)
        where TEntity : class, IContent
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }
            var repositoryType = assembly.GetTypes().Where(x => x.IsClass && typeof(IContentRepository<TEntity>).IsAssignableFrom(x)).FirstOrDefault();
            if (repositoryType == null)
            {
                throw new InvalidOperationException($"No repository found for {typeof(TEntity).Name}. Please create a class that implements IContentRepository<{typeof(TEntity).Name}>.");
            }
            services.AddTransient(typeof(IContentRepository<TEntity>), repositoryType);
            foreach (var @interface in repositoryType.GetInterfaces().Where(x => x.Name.ToLower().Contains("repository") && x != typeof(IContentRepository<TEntity>)))
            {
                services.AddTransient(@interface, repositoryType);
            }
            var contentLinkFinderType = assembly.GetTypes().Where(x => x.IsClass && typeof(IContentLinkFinder<TEntity>).IsAssignableFrom(x)).FirstOrDefault();
            if (contentLinkFinderType != null)
            {
                services.AddSingleton(typeof(IContentLinkFinder<TEntity>), contentLinkFinderType);
            }
            if (options != null)
            {
                services.PostConfigure(options);
            }
            if (editPageConfiguration != null)
            {
                services.PostConfigure<RobinatorOptions>(o => o.DefaultEditPages.Add(editPageConfiguration));
            }
            return services;
        }
    }
}
