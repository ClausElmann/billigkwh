using System;

namespace BilligKwhWebApp.Services.Interfaces
{
    public interface IUtcClock
    {
        DateTime Now();
        DateTime Today();
    }
}
