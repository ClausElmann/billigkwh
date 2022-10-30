using BilligKwhWebApp.Core.Domain;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.QueryWrappers
{
    public class UserWrapper
    {
        public int PseudoId { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
