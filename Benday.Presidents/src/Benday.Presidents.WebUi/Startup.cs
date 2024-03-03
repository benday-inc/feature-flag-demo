using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Benday.Presidents.WebUi.Data;
using Benday.Presidents.WebUi.Models;
using Benday.Presidents.WebUi.Services;
using Microsoft.AspNetCore.Http;
using Benday.Presidents.Api.Interfaces;
using Benday.Presidents.Api.Services;
using Benday.Presidents.Api.Features;
using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.DataAccess.SqlServer;
using Benday.Presidents.Common;

namespace Benday.Presidents.WebUi
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            RegisterTypes(services);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=President}/{action=Index}/{id?}");
            });
        }

        void RegisterTypes(IServiceCollection services)
        {
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            // services.AddTransient<IEmailSender, AuthMessageSender>();
            // services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IUsernameProvider, HttpContextUsernameProvider>();

            services.AddTransient<IFeatureManager, FeatureManager>();

            services.AddTransient<Api.Services.ILogger, Logger>();

            services.AddDbContext<PresidentsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("default")));

            services.AddTransient<IPresidentsDbContext, PresidentsDbContext>();

            services.AddTransient<IRepository<Person>, SqlEntityFrameworkPersonRepository>();
            services.AddTransient<IPresidentValidatorStrategy, PresidentValidatorStrategy>();

            services.AddTransient<IFeatureRepository, SqlEntityFrameworkFeatureRepository>();

            var tempServiceProvider = services.BuildServiceProvider();

            var features = tempServiceProvider.GetService<IFeatureManager>();

            if (features.FeatureUsageLogging == false)
            {
                services.AddTransient<IPresidentService, PresidentService>();
            }
            else
            {
                services.AddTransient<IPresidentService, PresidentServiceWithLogging>();
            }
        }
    }
}
