﻿using Autofac;
using DotNetClub.Core;
using DotNetClub.Data.EntityFramework;
using DotNetClub.Data.EntityFramework.Context;
using DotNetClub.Domain.Consts;
using DotNetClub.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Core.Model.Configuration;
using NLog.Extensions.Logging;
using DotNetClub.Core.Redis;

namespace DotNetClub.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddOptions();
            services.AddDbContext<ClubContext>(builder =>
            {
                builder.UseSqlServer(this.Configuration["ConnectionString"], options =>
                {
                    options.UseRowNumberForPaging();
                    options.MigrationsAssembly("DotNetClub.Web");
                });
            }, ServiceLifetime.Transient);

            services.Configure<RedisOptions>(Configuration.GetSection("Redis").Bind)
                .Configure<SiteConfiguration>(Configuration.GetSection("Site").Bind);

            services.AddScoped<Core.Security.SecurityManager>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInstance(this.Configuration).AsImplementedInterfaces();

            builder.RegisterType<RedisProvider>().As<IRedisProvider>().SingleInstance();

            builder.AddUnitOfWork(provider =>
            {
                provider.Register(new DotNetClub.Data.EntityFramework.ClubUnitOfWorkRegisteration());
            });

            builder.RegisterModule<CoreModule>()
                .RegisterModule<EntityFrameworkModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            app.UseExecuteTime();

            app.UseMvc();
        }
    }
}
