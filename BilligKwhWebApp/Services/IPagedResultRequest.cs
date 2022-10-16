using System.Collections.Generic;

namespace BilligKwhWebApp.Services
{
    /// <summary>
    /// Class to be used when arequesting paged data
    /// </summary>
    public interface IPagedResultRequest
    {
        /// <summary>
        /// Page number being requested. Default 0, meaning first page
        /// </summary>
        int Page { get; set; }
        /// <summary>
        /// How many records to return per page. Default 25
        /// </summary>
        int PageSize { get; set; }

        PagedResultSearchFilter Search { get; set; }

        /// <summary>
        /// Use this for specifying which columns should be sorted and in which order.
        /// </summary>
        IReadOnlyCollection<PageResultOrdering> Ordering { get; set; }
    }


    /// <summary>
    /// Defines an object for specifying search terms as either a regex and/or a value.
    /// </summary>
    public class PagedResultSearchFilter
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class PageResultOrdering
    {
        public string PropertyName { get; set; }
        /// <summary>
        /// ASC or DESC
        /// </summary>
        public string Direction { get; set; }
    }
}
