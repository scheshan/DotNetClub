using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure.Redis
{
    public interface IRedisProvider
    {
        IDatabase GetDatabase(int? db = -1);

        IServer GetServer();
    }
}
