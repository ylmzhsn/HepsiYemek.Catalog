
namespace HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Redis
{
    using HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Abstract;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Redis cache service.
    /// </summary>
    public class RedisCacheService : ICacheService
    {
        private RedisServer _redisServer;

        /// <summary>
        /// ctor
        /// </summary>
        public RedisCacheService(RedisServer redisServer)
        {
            _redisServer = redisServer;
        }

        /// <summary>
        /// Add new key to redis
        /// </summary>
        public void Add(string key, object data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            _redisServer.Database.StringSet(key, jsonData);
        }

        /// <summary>
        /// Sets time to live (cache expire)
        /// </summary>
        public bool SetTTL(string key, TimeSpan ttl)
        {
            return _redisServer.Database.KeyExpire(key, ttl);
        }

        public bool Any(string key)
        {
            return _redisServer.Database.KeyExists(key);
        }

        /// <summary>
        /// Is there any cache added with this key
        /// </summary>
        public T Get<T>(string key)
        {
            if (Any(key))
            {
                var jsonData = _redisServer.Database.StringGet(key);

                return JsonConvert.DeserializeObject<T>(jsonData);
            }

            return default;
        }

        /// <summary>
        /// Remove key from redis
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _redisServer.Database.KeyDelete(key);
        }

        /// <summary>
        /// Flush db
        /// </summary>
        public void Clear()
        {
            _redisServer.FlushDatabase();
        }
    }
}
