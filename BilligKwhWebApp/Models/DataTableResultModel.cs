using System.Collections;

namespace BilligKwhWebApp.Models
{
    /// <summary>
    /// Model used when returning data for a DataTable. We ensure that prop name casing is always camelCase when serializing an instance of this.
    /// This is in order to handle when returning dynamic table data where column names are not pre-defined in frontend. In these situations, when calling Json(), we
    /// must use the DefaultContractResolver with DefaultNamingStrategy to not alter the property name casing of the returned data objects. An example is when we return
    /// rows from an uploaded positive list.
    /// </summary>
    public class DataTableResultModel
    {
        public object ExtraData { get; set; }

        public IEnumerable Data { get; set; }

        public object Errors { get; set; }

        public int Total { get; set; }
    }
}
