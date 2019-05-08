using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Robinator.Core;
using Robinator.Example.Areas.Blog.Content;
using Robinator.Example.Areas.Blog.Models;
using Robinator.Example.Areas.News.Content;
using Robinator.Example.Areas.News.Models;

namespace Robinator.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("application"));
            services.AddRobinatorTypeDynamic(editPageConfiguration: new DefaultEditPageConfiguation<BlogPost>(x => new ContentHeaderViewModel
            {
                Id = x.Id,
                Text = x.Title,
            }));
            services.AddRobinatorTypeDynamic(editPageConfiguration: new DefaultEditPageConfiguation<NewsPost>("News", x => new ContentHeaderViewModel
            {
                Id = x.Id,
                Text = x.Title,
            }));

            services.AddMvc()
                .AddNewtonsoftJson();
            services.AddRobinatorDeafult().AddRobinatorDefaultEntityFramework(options => options.UseInMemoryDatabase("test"));
            services.AddRobinatorCKEditor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting(routes =>
            {
                routes.MapRazorPages();
                routes.MapApplication();
            });

            app.UseCookiePolicy();

            app.UseAuthorization();
        }
    }
}
