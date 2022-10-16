using System.Collections.Generic;
using MediatR;
using BilligKwhWebApp.Core.Domain;

namespace BilligKwhWebApp.Core.Infrastructure
{
    public abstract class AggregateRoot : BaseEntity
    {
        // Versioning..
        //public virtual int Version { get; protected set; }

        public List<INotification> DomainEvents = new List<INotification>();
        public List<INotification> IntegrationEvents = new List<INotification>();

        protected virtual void AddDomainEvent(INotification newEvent)
        {
            DomainEvents.Add(newEvent);
        }
        protected virtual void AddIntegrationEvent(INotification newEvent)
        {
            IntegrationEvents.Add(newEvent);
        }
        public virtual void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }
        public virtual void ClearIntegrationEvents()
        {
            IntegrationEvents.Clear();
        }
    }
}
