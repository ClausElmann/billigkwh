using BilligKwhWebApp.Core.Domain;
using MediatR;

namespace BilligKwhWebApp.Events
{
    public class CustomerUpdatedEvent : INotification
    {
        public CustomerUpdatedEvent(Kunde customer)
        {
            Customer = customer;
        }
        public Kunde Customer { get; }
    }
}
