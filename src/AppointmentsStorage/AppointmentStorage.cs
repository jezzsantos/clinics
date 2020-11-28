using AppointmentsApplication.Storage;
using AppointmentsDomain;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny.Primitives;
using Storage;
using Storage.Interfaces;

namespace AppointmentsStorage
{
    public class AppointmentStorage : IAppointmentStorage
    {
        private readonly IEventStreamStorage<AppointmentEntity> clinicEventStreamStorage;

        public AppointmentStorage(ILogger logger, IDomainFactory domainFactory,
            IEventStreamStorage<AppointmentEntity> eventStreamStorage,
            IRepository repository)
        {
            logger.GuardAgainstNull(nameof(logger));
            domainFactory.GuardAgainstNull(nameof(domainFactory));
            eventStreamStorage.GuardAgainstNull(nameof(eventStreamStorage));
            repository.GuardAgainstNull(nameof(repository));

            this.clinicEventStreamStorage = eventStreamStorage;
        }

        public AppointmentStorage(
            IEventStreamStorage<AppointmentEntity> clinicEventStreamStorage)
        {
            clinicEventStreamStorage.GuardAgainstNull(nameof(clinicEventStreamStorage));
            this.clinicEventStreamStorage = clinicEventStreamStorage;
        }

        public AppointmentEntity Load(Identifier id)
        {
            return this.clinicEventStreamStorage.Load(id);
        }

        public AppointmentEntity Save(AppointmentEntity clinic)
        {
            this.clinicEventStreamStorage.Save(clinic);
            return clinic;
        }
    }
}