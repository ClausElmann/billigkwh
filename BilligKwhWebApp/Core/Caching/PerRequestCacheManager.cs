using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace BilligKwhWebApp.Core.Caching
{
    public partial class PerRequestCacheManager : ICacheManager
    {
        // Props
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Ctor
        public PerRequestCacheManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        // Public Api
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
                items[key] = data;
        }

        public virtual bool IsSet(string key)
        {
            var items = GetItems();

            return items?[key] != null;
        }

        public virtual void Remove(string key)
        {
            var items = GetItems();

            items?.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Select(p => p.ToString()));
        }

        public virtual void Clear()
        {
            var items = GetItems();

            items?.Clear();
        }

        public virtual void Dispose()
        {
            //nothing special
        }



        // Internal
        protected virtual IDictionary<object, object> GetItems()
        {
            return _httpContextAccessor.HttpContext?.Items;
        }
    }

}
