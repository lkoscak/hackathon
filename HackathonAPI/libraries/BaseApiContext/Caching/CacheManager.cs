using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace WebApi.Core.Caching
{
    public class CacheManager : ICacheManager
    {
        private const string baseDomain = "fleet_webapi_";
        private const string defaultDomain = baseDomain + "default";
        private const string KeySeparator = "::";
        private static string TokenIdleTime { get { return System.Configuration.ConfigurationManager.AppSettings["TokenCacheIdleTime"]; } }

        private MemoryCache cache = MemoryCache.Default;

        public T fromCache<T>(string group, string key)
        {
            object result = cache.Get(CombinedKey(key, group != null ? baseDomain + group : defaultDomain));
            T r = (T)result;

            return r;
        }

        public object fromCacheToken(string token)
        {
            object result = cache.Get(CombinedKey(token, getCacheRegionName(CacheType.Token)));
            //string r = (string)result;

            return result;
        }

        /// <summary>
        /// Store to cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        /// <param name="expiration">ms</param>
        public void toCache(string group, string key, object value, CacheType cacheType, long expiration = 24 * 60 * 60 * 1000)
        {
            cache.Add(CombinedKey(key, group != null ? baseDomain + group : defaultDomain), value, getCachePolicy(cacheType));
        }

        public void toCacheToken(string token, object session)
        {
            cache.Add(CombinedKey(token, getCacheRegionName(CacheType.Token)), session, getCachePolicy(CacheType.Token));
        }

        public void removeCache(string group, string key)
        {
            cache.Remove(CombinedKey(key, group));
        }

        public void removeCacheToken(string token)
        {
            cache.Remove(CombinedKey(token, getCacheRegionName(CacheType.Token)));
        }

        private CacheItemPolicy getCachePolicy(CacheType cacheType, long expiration = 24 * 60 * 60 * 1000)
        {
            var cachePolicty = new CacheItemPolicy();
            switch (cacheType)
            {
                case CacheType.Token:
                    cachePolicty.SlidingExpiration = TimeSpan.FromMilliseconds(double.Parse(TokenIdleTime));
                    //cachePolicty.AbsoluteExpiration = DateTime.Now.AddSeconds(300);
                    break;
                case CacheType.AbsoluteExpiration:
                    cachePolicty.AbsoluteExpiration = DateTime.Now.AddMilliseconds(expiration);
                    break;
                case CacheType.SlidingExpiration:
                    cachePolicty.SlidingExpiration = TimeSpan.FromMilliseconds(expiration);
                    break;
                default:
                    cachePolicty.AbsoluteExpiration = DateTime.Now.AddMilliseconds(expiration);
                    break;
            }

            return cachePolicty;
        }

        private string getCacheRegionName(CacheType cacheType)
        {
            string key = "";
            switch (cacheType)
            {
                case CacheType.Token:
                    key = baseDomain + "_token_";
                    break;
                default:
                    key = defaultDomain;
                    break;
            }

            return key;
        }

        private static string CombinedKey(object key, string domain)
        {
            return string.Format("{0}{1}{2}", key, KeySeparator, domain);
        }
    }

    public enum CacheType
    {
        Token,
        AbsoluteExpiration,
        SlidingExpiration
    }
}