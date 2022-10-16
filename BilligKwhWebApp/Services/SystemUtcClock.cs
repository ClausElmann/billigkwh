using BilligKwhWebApp.Services.Interfaces;
using System;

namespace BilligKwhWebApp.Services
{
    public class SystemUtcClock : IUtcClock
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }

        public DateTime Today()
        {
            return DateTime.Today.ToUniversalTime();
        }
    }
}
