namespace BilligKwhWebApp.Models
{
    /// <summary>
    /// Common, generic model used for dropdown/select values in frontend. 
    /// </summary>
    public class SelectListItemModel<T>
    {
        /// <summary>
        /// String or number used for the dropdown value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// The name of the select list item to display in view 
        /// </summary>
        public string DisplayName { get; set; }

    }
}
