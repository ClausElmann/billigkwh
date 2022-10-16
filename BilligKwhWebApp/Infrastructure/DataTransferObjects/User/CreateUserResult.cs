using System.Collections.Generic;

namespace BilligKwhWebApp.Infrastructure.DataTransferObjects.User
{
    public class CreateUserResult
    {
        public int Id { get; set; }
        public IEnumerable<UserCreationWarning> Warnings { get; set; }

        public CreateUserResult(int id, IEnumerable<UserCreationWarning> warnings)
        {
            Id = id;
            Warnings = warnings;
        }
    }
}
