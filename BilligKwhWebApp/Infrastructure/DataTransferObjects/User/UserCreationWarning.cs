using System.Collections.Generic;

namespace BilligKwhWebApp.Infrastructure.DataTransferObjects.User
{
    public class UserCreationWarning
    {
        public string LocaleStringResource { get; set; }
        public IEnumerable<string> Parameters { get; set; } 
    }
}
