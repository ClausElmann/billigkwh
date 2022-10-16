using BilligKwhWebApp.Services.Interfaces;
using System;

namespace BilligKwhWebApp.Services
{
    public class GloballyUniqueIdentifier : IGloballyUniqueIdentifier
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
