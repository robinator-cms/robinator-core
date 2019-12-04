using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Robinator.Core;
using Robinator.Example.Areas.Blog.Models;
using Robinator.Example.Areas.Identity.Services;
using Robinator.Example.Areas.News.Models;
using System;

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

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddRobinatorTypeDynamic(editPageConfiguration: new DefaultEditPageConfiguation<BlogPost>(x => new ContentHeaderViewModel
            {
                Id = x.Id,
                Text = x.Title,
            }));
            services.AddRobinatorTypeDynamic(editPageConfiguration: new DefaultEditPageConfiguation<NewsPost>(x => new ContentHeaderViewModel
            {
                Id = x.Id,
                Text = x.Title,
            }, name: "News", summaryPartialName: "News.Summary", link: "news"));

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    //options.Conventions.AuthorizeAreaFolder("RobinatorAdmin", "/");
                })
                .AddNewtonsoftJson();
            services.AddRobinatorDeafult()
                    .AddRobinatorDefaultEntityFramework(options => options.UseInMemoryDatabase("test"));
            services.AddRobinatorCKEditor();
            services.AddRobinatorLocalFiles(options =>
            {
                options.RootPath = "wwwroot/public";
                options.PublicPath = "/public";
            });
            services.AddRobinatorFileServiceDefaultUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
