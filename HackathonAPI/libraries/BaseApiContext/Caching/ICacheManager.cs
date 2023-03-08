using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Core.Caching
{
    public interface ICacheManager
    {
        T fromCache<T>(string group, string key);
        object fromCacheToken(string token);
        void toCache(string group, string key, object value, CacheType cacheType, long expiration = 24 * 60 * 60 * 1000);
        void toCacheToken(string token, object session);
        void removeCache(string group, string key);
        void removeCacheToken(string token);
    }
}
