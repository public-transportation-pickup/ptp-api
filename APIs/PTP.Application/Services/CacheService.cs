using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using PTP.Application.Services.Interfaces;
using PTP.Domain.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private static readonly ConcurrentDictionary<string, bool> CachKeys = new();
        private readonly IDistributedCache _distributedCache;

        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            string? cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (cacheValue is null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(cacheValue);

        }

        public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            T? cachedValue = await GetAsync<T>(key, cancellationToken);
            if (cachedValue is not null)
            {
                return cachedValue;
            }

            cachedValue = await factory();
            await SetAsync(key, cachedValue, cancellationToken);
            return cachedValue;
        }

        public async Task<List<T>?> GetByPrefixAsync<T>(string prefixKey, CancellationToken cancellationToken = default) where T : class
        {
            List<T> cachedValue = new List<T>();
            foreach (var key in CachKeys.Keys)
            {
                if (key.StartsWith(prefixKey))
                {
                    var value = await GetAsync<T>(key, cancellationToken);
                    if (value is not null)
                    {
                        cachedValue.Add(value);
                    }
                }
            }
            return cachedValue;
        }

        public Task<List<T>?> GetByPrefixAsync<T>(string prefixKey, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            throw new NotImplementedException();
        }

        public bool IsConnected() => _distributedCache == null ? true : false;


        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            CachKeys.Remove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            IEnumerable<Task> tasks = CachKeys
                .Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => RemoveAsync(k, cancellationToken));

            await Task.WhenAll(tasks);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            string cacheValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

            CachKeys.TryAdd(key, false);

        }

        public async Task SetByPrefixAsync<T>(string prefixKey, List<T> values, CancellationToken cancellationToken = default) where T : BaseEntity
        {
            foreach (var item in values)
            {
                var key = prefixKey+ item.Id;
                await SetAsync<T>(key, item, cancellationToken);
            }
        }
    }
}
