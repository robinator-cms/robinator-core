using Robinator.Core;
using Robinator.Core.Areas.Pages;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
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
    }
}
