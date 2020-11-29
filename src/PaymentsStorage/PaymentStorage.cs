using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using PaymentsApplication.Storage;
using PaymentsDomain;
using QueryAny.Primitives;
using Storage;
using Storage.Interfaces;

namespace PaymentsStorage
{
    public class PaymentStorage : IPaymentStorage
    {
        private readonly IEventStreamStorage<PaymentEntity> eventingStorage;

        public PaymentStorage(ILogger logger, IDomainFactory domainFactory,
            IEventStreamStorage<PaymentEntity> eventStreamStorage,
            IRepository repository)
        {
            logger.GuardAgainstNull(nameof(logger));
            domainFactory.GuardAgainstNull(nameof(domainFactory));
            eventStreamStorage.GuardAgainstNull(nameof(eventStreamStorage));
            repository.GuardAgainstNull(nameof(repository));

            this.eventingStorage = eventStreamStorage;
        }

        public PaymentStorage(IEventStreamStorage<PaymentEntity> eventingStorage)
        {
            eventingStorage.GuardAgainstNull(nameof(eventingStorage));
            this.eventingStorage = eventingStorage;
        }

        public PaymentEntity Save(PaymentEntity person)
        {
            this.eventingStorage.Save(person);
            return person;
        }
    }
}