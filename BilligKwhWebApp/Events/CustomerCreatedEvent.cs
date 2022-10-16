using BilligKwhWebApp.Core.Domain;
using MediatR;

namespace BilligKwhWebApp.Events
{
    public class CustomerCreatedEvent : INotification
    {
        public CustomerCreatedEvent(Kunde customer)
        {
            Customer = customer;
        }
        public Kunde Customer { get; }
    }
}
