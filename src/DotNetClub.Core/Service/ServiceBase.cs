using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DotNetClub.Domain.Consts;
using DotNetClub.Core.Model.Configuration;
using Shared.Infrastructure.Redis;
using Microsoft.Extensions.Options;

namespace DotNetClub.Core.Service
{
    public abstract class ServiceBase
    {
        protected IServiceProvider ServiceProvider { get; private set; }

        protected IUnitOfWorkProvider UnitOfWorkProvider
        {
            get
            {
                return this.ServiceProvider.GetService<IUnitOfWorkProvider>();
            }
        }

        protected IOptions<SiteConfiguration> SiteConfiguration
        {
            get
            {
                return this.ServiceProvider.GetService<IOptions<SiteConfiguration>>();
            }
        }

        protected IRedisProvider RedisProvider
        {
            get
            {
                return this.ServiceProvider.GetService<IRedisProvider>();
            }
        }

        protected Security.SecurityManager SecurityManager
        {
            get
            {
                return this.ServiceProvider.GetService<Security.SecurityManager>();
            }
        }

        public ServiceBase(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.ServiceProvider = serviceProvider;
        }

        protected IUnitOfWork CreateUnitOfWork()
        {
            return this.UnitOfWorkProvider.CreateUnitOfWork(UnitOfWorkNames.EntityFramework);
        }
    }
}
