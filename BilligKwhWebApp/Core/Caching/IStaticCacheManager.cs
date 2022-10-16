namespace BilligKwhWebApp.Core.Caching.Interfaces
{
    public interface IStaticCacheManager : ICacheManager
    {
        void ClearCache();
    }
}
