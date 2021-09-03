
namespace HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Redis
{
    using System;
    using System.Text;
    
    using Newtonsoft.Json;
    using StackExchange.Redis;
    

    using HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Abstract;

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
            if (data == null)
            {
                return;
            }

            byte[] entry = Serialize(data);

            _redisServer.Database.StringSet(key, entry);
        }

        /// <summary>
        /// Serialize item to byte array
        /// </summary>
        protected virtual byte[] Serialize<T>(T item)
        {
            string jsonString = JsonConvert.SerializeObject(item);

            return Encoding.UTF8.GetBytes(jsonString);
        }

        /// <summary>
        /// Deserialize byte array to T object
        /// </summary>
        protected virtual T Deserialize<T>(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return default(T);
            }

            string jsonString = Encoding.UTF8.GetString(serializedObject);

            return JsonConvert.DeserializeObject<T>(jsonString);
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
                RedisValue redisValue = _redisServer.Database.StringGet(key);

                return Deserialize<T>(redisValue);
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
