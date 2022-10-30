using BilligKwhWebApp.Core.Domain;
using MediatR;

namespace BilligKwhWebApp.Events
{
    public class CustomerDeletedEvent : INotification
    {
        public CustomerDeletedEvent(Customer customer)
        {
            Customer = customer;
        }
        public Customer Customer { get; }
    }
}
