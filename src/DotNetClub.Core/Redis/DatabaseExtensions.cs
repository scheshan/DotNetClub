using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetClub.Core.Redis
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireDate"></param>
        public static void JsonSet<T>(this IDatabase redis, RedisKey key, T value, TimeSpan? expireDate)
        {
            string json = JsonConvert.SerializeObject(value);

            redis.StringSet(key, json, expireDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T JsonGet<T>(this IDatabase redis, RedisKey key)
        {
            RedisValue value = redis.StringGet(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public static void JsonHashSet<T>(this IDatabase redis, RedisKey key, RedisValue field, T value)
        {
            string json = JsonConvert.SerializeObject(value);

            redis.HashSet(key, field, json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static T JsonHashGet<T>(this IDatabase redis, RedisKey key, RedisValue field)
        {
            RedisValue value = redis.HashGet(key, field);

            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static List<T> JsonHashGet<T>(this IDatabase redis, RedisKey key, RedisValue[] fields)
        {
            RedisValue[] values = redis.HashGet(key, fields);

            return values.Where(t => t.HasValue).Select(t => JsonConvert.DeserializeObject<T>(t)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        public static void JsonHashSetAll<T>(this IDatabase redis, RedisKey key, Dictionary<string, T> fields)
        {
            HashEntry[] hashFields = fields.Select(t => 
                new HashEntry(
                    t.Key, 
                    JsonConvert.SerializeObject(t.Value)
                )
            ).ToArray();

            redis.HashSet(key, hashFields);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redis"></param>
        /// <param name="key"></param>
        public static Dictionary<RedisValue, T> JsonHashGetAll<T>(this IDatabase redis, RedisKey key)
        {
            HashEntry[] hashFields = redis.HashGetAll(key);

            Dictionary<RedisValue, T> result = new Dictionary<RedisValue, T>();
            foreach (HashEntry hashField in hashFields)
            {
                result.Add(hashField.Name, JsonConvert.DeserializeObject<T>(hashField.Value));
            }

            return result;
        }
    }
}
