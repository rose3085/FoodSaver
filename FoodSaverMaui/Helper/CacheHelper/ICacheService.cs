using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Helper.CacheHelper
{
    public interface ICacheService
    {
        Task AddOrUpdateCache(string key, object value);
        Task<T> GetFromCache<T>(string key);

        Task RemoveFromCache(string key);
        Task Clear();
    }
}
