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
using DotNetClub.Web.Middlewares;
using NLog.Extensions.Logging;
using Autofac;
using Share.Infrastructure;
using DotNetClub.Domain.Consts;
using DotNetClub.Data.EntityFramework;

namespace DotNetClub.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<Core.Security.SecurityManager>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInstance(this.Configuration).AsImplementedInterfaces();

            builder.AddUnitOfWork(uowBuilder =>
            {
                uowBuilder.AddEntityFramework<Data.EntityFramework.Context.ClubContext>(UnitOfWorkNames.EntityFramework, null);
            });

            builder.AddEntityFrameworkRepository();
            builder.AddCoreServices();

            var redisConfiguration = builder.AddConfiguration<Core.Model.Configuration.RedisConfiguration>(this.Configuration.GetSection("Redis"));
            var siteConfiguration = builder.AddConfiguration<Core.Model.Configuration.SiteConfiguration>(this.Configuration.GetSection("Site"));
            builder.AddRedis(redisConfiguration.Host, redisConfiguration.Port, redisConfiguration.Password, redisConfiguration.Db);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddNLog();
            //env.ConfigureNLog("nlog.config");

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

            app.UseExecuteTime();

            app.UseMvc();
        }
    }
}
