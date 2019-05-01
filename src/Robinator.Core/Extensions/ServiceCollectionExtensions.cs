using System;
using Robinator.Core;
using Robinator.Core.Areas.Pages;
using Robinator.Core.Areas.RobinatorAdmin.Pages.ViewModels;
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
                    Id = (x as Page).Id,
                    Text = (x as Page).Title,
                }));
            });
        }
    }
}
