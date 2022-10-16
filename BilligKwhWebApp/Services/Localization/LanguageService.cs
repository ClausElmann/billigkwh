using System.Collections.Generic;
using System.Linq;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Caching;
using BilligKwhWebApp.Core.Interfaces;

namespace BilligKwhWebApp.Services.Localization
{
    public partial interface ILanguageService
    {
        IList<Language> GetAllLanguages();
    }

    public class LanguageService : ILanguageService
    {
        // Props
        private const string _LANGUAGES_ALL_KEY = "BilligKwhWebApp.language.all";
       
        private readonly IBaseRepository _baseRepository;
        private readonly IStaticCacheManager _cacheManager;

        
        // Ctor
        public LanguageService(IStaticCacheManager cacheManager,
			IBaseRepository baseRepository)
        {
            _cacheManager = cacheManager;
            _baseRepository = baseRepository;
        }

        public virtual IList<Language> GetAllLanguages()
        {
			var key = _LANGUAGES_ALL_KEY;
            return _cacheManager.Get(key, CacheTimeout.TenHours, () => 
			{
				using (var connection = ConnectionFactory.GetOpenConnection()) 
				{
					return _baseRepository.Query<Language>("SELECT * FROM dbo.Languages ORDER BY DisplayOrder").ToList();
				}
			});
		}
    }
}
