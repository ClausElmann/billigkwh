using BilligKwhWebApp.Core.Domain;
using MediatR;

namespace BilligKwhWebApp.Events
{
    public class CustomerCreatedEvent : INotification
    {
        public CustomerCreatedEvent(Customer customer)
        {
            Customer = customer;
        }
        public Customer Customer { get; }
    }
}
