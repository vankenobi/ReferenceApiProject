using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Reference.Api.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<bool> Exists(string key)
        {
            try
            {
                var item = await _distributedCache.GetAsync(key);
                if (item != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T?> Get<T>(string key) where T : class
        {
            try
            {
                string json;
                T? item = null; // Başlangıçta null olarak tanımlıyoruz

                var itemFromCache = await _distributedCache.GetAsync(key);
                if (itemFromCache != null)
                {
                    json = Encoding.UTF8.GetString(itemFromCache);
                    item = JsonConvert.DeserializeObject<T>(json);
                }

                return item;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Remove(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Set<T>(string key, T value, DateTimeOffset expirationTime)
        {
            try
            {
                string json = JsonConvert.SerializeObject(value);
                byte[] serializedValue = Encoding.UTF8.GetBytes(json);

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = expirationTime
                };

                await _distributedCache.SetAsync(key, serializedValue, options);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

