using BilligKwhWebApp.Core.Domain;
using MediatR;

namespace BilligKwhWebApp.Events
{
    public class CustomerDeletedEvent : INotification
    {
        public CustomerDeletedEvent(Kunde customer)
        {
            Customer = customer;
        }
        public Kunde Customer { get; }
    }
}
