using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using BilligKwhWebApp.Core.Caching.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace BilligKwhWebApp.Core.Caching
{
    public partial class MemoryCacheManager : IStaticCacheManager
    {
        #region Fields

        private readonly IMemoryCache _cache;

        protected CancellationTokenSource _cancellationTokenSource;

        protected static readonly ConcurrentDictionary<string, bool> _allKeys;

        #endregion

        #region Ctor

        static MemoryCacheManager()
        {
            _allKeys = new ConcurrentDictionary<string, bool>();
        }

        public MemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        #endregion

        // Internals
        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int cacheTime)
        {
            var options = new MemoryCacheEntryOptions()
                // add cancellation token for clear cache
                .AddExpirationToken(new CancellationChangeToken(_cancellationTokenSource.Token))
                //add post eviction callback
                .RegisterPostEvictionCallback(PostEviction);

            //set cache time
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime);

            return options;
        }

        protected string AddKey(string key)
        {
            _allKeys.TryAdd(key, true);
            return key;
        }

        protected string RemoveKey(string key)
        {
            TryRemoveKey(key);
            return key;
        }

        protected void TryRemoveKey(string key)
        {
            //try to remove key from dictionary
            if (!_allKeys.TryRemove(key, out bool _))
                //if not possible to remove key from dictionary, then try to mark key as not existing in cache
                _allKeys.TryUpdate(key, false, false);
        }

        public void ClearCache()
        {

            foreach (var key in _allKeys.Select(p => p.Key).ToList())
            {
                _cache.Remove(key);
            }
        }

        private void ClearKeys()
        {
            foreach (var key in _allKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
            {
                RemoveKey(key);
            }
        }


        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            //if cached item just change, then nothing doing
            if (reason == EvictionReason.Replaced)
                return;

            //try to remove all keys marked as not existing
            ClearKeys();

            //try to remove this key from dictionary
            TryRemoveKey(key.ToString());
        }

        // Public Api
        public virtual T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data != null)
            {
                _cache.Set(AddKey(key), data, GetMemoryCacheEntryOptions(cacheTime));
            }
        }

        public virtual bool IsSet(string key)
        {
            return _cache.TryGetValue(key, out object _);
        }

        public virtual void Remove(string key)
        {
            _cache.Remove(RemoveKey(key));
        }

        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, _allKeys.Where(p => p.Value).Select(p => p.Key));
        }

        public virtual void Clear()
        {
            //send cancellation request
            _cancellationTokenSource.Cancel();

            //releases all resources used by this cancellation token
            _cancellationTokenSource.Dispose();

            //recreate cancellation token
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Dispose cache manager
        /// </summary>
        public virtual void Dispose()
        {
            //nothing special
        }
    }
}
