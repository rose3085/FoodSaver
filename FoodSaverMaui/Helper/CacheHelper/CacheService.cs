using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Helper.CacheHelper
{
    public class CacheService : ICacheService
    {

        private readonly ConcurrentDictionary<string, object> _cache = new();

        public async Task AddOrUpdateCache(string key, object value)
        {
            if (value != null && key != null)
            {
                _cache[key] = value;
            }
        }
    

        public async Task Clear()
        {
            _cache.Clear();
        }

        public async Task<T> GetFromCache<T>(string key)
        {
            try {

                if (key != null)
                {
                    return _cache.TryGetValue(key, out object value) ? (T?)value : default;
                }
                else
                {
                    return default(T);
                }
            
            } catch { return default(T); }
        }

        public async Task RemoveFromCache(string key)
        {
            if (key != null)
            {
                _cache.TryRemove(key, out _);
            }
        }
    }
}
