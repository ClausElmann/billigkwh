using BilligKwhWebApp.Services;
using System.Collections.Generic;

namespace BilligKwhWebApp.Infrastructure.DataTransferObjects.Common
{
    public class LanguageResourcesDto : IPagedResultRequest
    {
        /// <summary>
        /// Page number being requested. Default 0, meaning first page
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// How many records to return per page. Default 25
        /// </summary>
        public int PageSize { get; set; } = 25;

        public PagedResultSearchFilter Search { get; set; }

        /// <summary>
        /// Use this for specifying which columns should be sorted and in which order.
        /// </summary>
        public IReadOnlyCollection<PageResultOrdering> Ordering { get; set; }

        public string SearchResourceName { get; set; }
        public string SearchResourceValue { get; set; }

    }
}
