using System;
using ApplicationServices;
using AppointmentsDomain;
using Domain.Interfaces.Entities;

namespace InfrastructureServices.Eventing.Notifications
{
    public class AppointmentDomainEventPublisher : IDomainEventPublisher
    {
        public Type EntityType => typeof(AppointmentEntity);

        public IChangeEvent Publish(IChangeEvent originalEvent)
        {
            return originalEvent;
        }
    }
}