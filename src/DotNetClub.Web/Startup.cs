using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DotNetClub.Core.Data;
using DotNetClub.Core;
using Newtonsoft.Json.Serialization;

namespace DotNetClub.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(this.Configuration);

            services.AddDataProtection();
            // Add framework services.
            services.AddDbContext<ClubContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
                b => 
                {
                    b.MigrationsAssembly("DotNetClub.Web");
                    b.UseRowNumberForPaging();
                })
            );

            services.AddMvc().AddJsonOptions(option=>
            {
                option.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH;mm:ss";
            });

            services.AddCoreServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseClientManagerInitializer();

            app.UseBrowserLink();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc();
        }
    }
}
