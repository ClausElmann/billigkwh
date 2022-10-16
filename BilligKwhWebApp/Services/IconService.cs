using System.Collections.Generic;
using System.Linq;
using Dapper;
using BilligKwhWebApp.Core.Caching;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Enums;

namespace BilligKwhWebApp.Services
{
    public class IconService : IIconService
    {
        // props
        private readonly IBaseRepository _baseRepository;
        private readonly IStaticCacheManager _cacheManager;


        // Ctor
        public IconService(IBaseRepository baseRepository, IStaticCacheManager cacheManager)
        {
            _baseRepository = baseRepository;
            _cacheManager = cacheManager;
        }

        ///All private/protected methods should be in Utilities...
        #region Private
        #endregion

        ///All public methods should be in Methods...
        #region Public

        public IList<Icon> GetIconsByType(IconTypeEnum iconType, int? customerId)
        {
            var sql = @" 
                    SELECT  Id, Name, Url, Tooltip, IconType, CustomerId
                    FROM dbo.Icons
                    WHERE CustomerId IS NULL OR CustomerId = @CustomerId
                    ";

            if (customerId.HasValue)
                return _baseRepository.Query<Icon>(sql, new { CustomerId = customerId }).ToList();


            var iconList = _cacheManager.Get(CacheKeys.Icons, CacheTimeout.VeryLong, () =>
            {
                using var connection = ConnectionFactory.GetOpenConnection();
                return connection.Query<Icon>(sql, new { CustomerId = customerId }).ToList();
            }).ToList();

            return iconList.Where(w => w.IconType == iconType.ToString()).ToList();
        }

        public Icon UpdateIcon(Icon icon)
        {
            if (icon is null)
                throw new System.ArgumentNullException(nameof(icon));
            if (icon.Id == 0)
                _baseRepository.Insert(icon);
            else
                _baseRepository.Update(icon);

            //Clear cache by removing key... new cache will be generated on new get.
            _cacheManager.Remove(CacheKeys.Icons);
            return icon;
        }

        public void DeleteIcon(Icon icon)
        {
            _baseRepository.Delete(icon);

            //Clear cache by removing key... new cache will be generated on new get.
            _cacheManager.Remove(CacheKeys.Icons);
        }

        public void DeleteIconById(int iconId)
        {
            _baseRepository.Execute("DELETE FROM dbo.Icons WHERE Id = @iconId ", new { iconId });

            //Clear cache by removing key... new cache will be generated on new get.
            _cacheManager.Remove(CacheKeys.Icons);
        }
        #endregion
    }
}
