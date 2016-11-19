using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Extensions
{
    public static class RedisDatabaseExtensions
    {
        public static List<T> JsonHashGet<T>(this IDatabase redis, RedisKey key, RedisValue[] fields)
        {
            RedisValue[] values = redis.HashGet(key, fields.Distinct().ToArray());

            return values.Where(t => t.HasValue).Select(t => JsonConvert.DeserializeObject<T>(t)).ToList();
        }
    }
}
