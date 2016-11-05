using Autofac;
using Microsoft.EntityFrameworkCore;
using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddUnitOfWork(this ContainerBuilder builder, Action<UnitOfWorkBuilder> optionsAction = null)
        {
            builder.RegisterType<UnitOfWorkProvider>().SingleInstance().AsImplementedInterfaces();
            builder.RegisterType<UnitOfWork.EntityFramework.EntityFrameworkUnitOfWork>();

            var unitOfWorkBuilder = new UnitOfWorkBuilder(builder);
            optionsAction?.Invoke(unitOfWorkBuilder);
            builder.RegisterInstance(unitOfWorkBuilder);

            return builder;
        }

        public static ContainerBuilder AddRedis(this ContainerBuilder builder, string host, int port = 6379, string password = null, int db = -1)
        {
            var redisProvider = new Redis.RedisProvider(host, port, password, db);
            builder.RegisterInstance(redisProvider).As<Redis.IRedisProvider>();

            return builder;
        }
    }
}
