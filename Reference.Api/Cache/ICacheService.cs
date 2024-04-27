using System;
namespace Reference.Api.Cache
{
	public interface ICacheService 
	{
        Task<T?> Get<T>(string key) where T : class;
        Task Set<T>(string key, T value, DateTimeOffset expirationTime);
        Task Remove(string key);
        Task<bool> Exists(string key);
    }
}

