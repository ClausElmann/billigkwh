namespace BilligKwhWebApp.Models
{
    public class UserRoleModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Name as used in database
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name to be used in frontend. The display value
        /// </summary>
        public string NameLocalized { get; set; }

        public string Description { get; set; }

        public bool PublicVisible { get; set; }

        public string Category { get; set; }
    }
}
