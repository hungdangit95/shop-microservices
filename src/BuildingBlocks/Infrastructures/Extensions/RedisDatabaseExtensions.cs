using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
namespace Infrastructures.Extensions
{
    public static class RedisDatabaseExtensions
    {
        public static async Task<T> GetAsync<T>(this IDatabase redis, string key) where T : class
        {
            var data = (await redis.StringGetAsync(key)).ToString();
            if (!string.IsNullOrWhiteSpace(data))
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            return null;
        }

        public static async Task<List<T>> GetAsync<T>(this IDatabase redis, string[] keys) where T : class
        {
            var keyList = keys.Select(x => new RedisKey(x)).ToArray();
            var values = await redis.StringGetAsync(keyList);
            var result = values?.Select(x => JsonConvert.DeserializeObject<T>(x)).ToList();
            return result;
        }

        public static async Task<List<T>> HGetAllAsListAsync<T>(this IDatabase redis, string key) where T : class
        {
            var data = await redis.HashGetAllAsync(key);
            var result = data?.Select(x => JsonConvert.DeserializeObject<T>(x.Value)).ToList();
            return result;
        }

        public static async Task<Dictionary<string, T?>> HGetAllAsDictionaryAsync<T>(this IDatabase redis, string key) where T : class
        {
            var data = await redis.HashGetAllAsync(key);
            var result = data?.ToDictionary(x => x.Name.ToString(), x => JsonConvert.DeserializeObject<T>(x.Value));
            return result;
        }

        public static async Task<T> HGetAsync<T>(this IDatabase redis, string key, string field) where T : class
        {
            var data = (await redis.HashGetAsync(key, field)).ToString();
            if (!string.IsNullOrWhiteSpace(data))
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            return null;
        }

        public static async Task<List<T>> HGetAsync<T>(this IDatabase redis, string key, string[] fields) where T : class
        {
            var result = new List<T>();
            var fieldList = fields.Select(x => new RedisValue(x)).ToArray();
            var values = await redis.HashGetAsync(key, fieldList);
            foreach (var value in values)
            {
                if (!value.IsNullOrEmpty)
                {
                    result.Add(JsonConvert.DeserializeObject<T>(value));
                }
            }
            return result;
        }

        public static async Task<List<T>> HGetAsync<T>(this IDatabase redis, string key, int?[] fields) where T : class
        {
            var result = new List<T>();
            var fieldList = fields.Select(x => new RedisValue(x.ToString())).ToArray();
            var values = await redis.HashGetAsync(key, fieldList);
            foreach (var value in values)
            {
                if (!value.IsNullOrEmpty)
                {
                    result.Add(JsonConvert.DeserializeObject<T>(value));
                }
            }
            return result;
        }

        public static async Task<bool> JsonSetAsync(this IDatabase redis, string key, object obj)
        {
            var data = JsonConvert.SerializeObject(obj);
            return await redis.StringSetAsync(key, data);
        }
       
        public static async Task<bool> HSetAsync(this IDatabase redis, string key, string field, object obj)
        {
            var data = JsonConvert.SerializeObject(obj);
            return await redis.HashSetAsync(key, field, data);
        }

        public static async Task HSetAsync<T>(this IDatabase redis, string key, Dictionary<string, T> data)
        {
            var values = data.Select(x => new HashEntry(x.Key, JsonConvert.SerializeObject(x.Value))).ToArray();
            await redis.HashSetAsync(key, values);
        }

        public static async Task<long> HDeleteAsync(this IDatabase redis, string key, string[] fields)
        {
            var values = fields?.Select(x => new RedisValue(x)).ToArray();
            return await redis.HashDeleteAsync(key, values);
        }

        public static async Task<bool> HDeleteAsync(this IDatabase redis, string key)
        {
            return await redis.KeyDeleteAsync(key);
        }
    }
}
