using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Enums;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services
{
    public interface IIconService
    {

        /// <summary>
        /// Gets a cached list of icons filtered by iconType and optionally customer id. 
        /// </summary>
        IList<Icon> GetIconsByType(IconTypeEnum iconType, int? customerId);

        /// <summary>
        /// Use Update to create and update, this function also clears cache
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        Icon UpdateIcon(Icon icon);

        /// <summary>
        /// Deletes icon by given icon object and clears cache
        /// </summary>
        /// <param name="icon"></param>
        void DeleteIcon(Icon icon);

        /// <summary>
        /// Deletes icon by ID and clears cache 
        /// </summary>
        /// <param name="iconId"></param>
        void DeleteIconById(int iconId);
    }
}
