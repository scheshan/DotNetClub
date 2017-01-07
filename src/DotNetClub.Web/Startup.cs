using Autofac;
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
using Shared.Infrastructure.Redis;
using DotNetClub.Core.Model.Configuration;
using NLog.Extensions.Logging;
using Shared.Infrastructure.UnitOfWork;

namespace DotNetClub.Web
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets();

            Configuration = builder.Build();
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

            RedisOptions redisOptions = new RedisOptions();
            Configuration.GetSection("Redis").Bind(redisOptions);

            builder.AddRedis(redisOptions)
                .AddUnitOfWork(unitOfWorkProvider =>
                {
                    unitOfWorkProvider.AddEntityFramework<ClubContext>(UnitOfWorkNames.EntityFramework, repositoryContainer =>
                    {
                        repositoryContainer.RegisterModule<EntityFrameworkModule>();
                    });
                });

            builder.RegisterModule<CoreModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");

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
