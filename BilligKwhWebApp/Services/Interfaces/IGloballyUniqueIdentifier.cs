using System;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface IGloballyUniqueIdentifier
    {
        public Guid NewGuid();
    }
}
