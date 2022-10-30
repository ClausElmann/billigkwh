using BilligKwhWebApp.Core.Domain;
using MediatR;

namespace BilligKwhWebApp.Events
{
    public class CustomerUpdatedEvent : INotification
    {
        public CustomerUpdatedEvent(Customer customer)
        {
            Customer = customer;
        }
        public Customer Customer { get; }
    }
}
