using System;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using PaymentsApplication.ReadModels;
using PaymentsDomain;
using QueryAny.Primitives;
using Storage;
using Storage.Interfaces.ReadModels;

namespace PaymentsStorage
{
    public class PaymentEntityReadModelProjection : IReadModelProjection
    {
        private readonly ILogger logger;
        private readonly IReadModelStorage<Payment> paymentStorage;

        public PaymentEntityReadModelProjection(ILogger logger, IRepository repository)
        {
            logger.GuardAgainstNull(nameof(logger));
            repository.GuardAgainstNull(nameof(repository));

            this.logger = logger;
            this.paymentStorage = new GeneralReadModelStorage<Payment>(logger, repository);
        }

        public Type EntityType => typeof(PaymentEntity);

        public bool Project(IChangeEvent originalEvent)
        {
            switch (originalEvent)
            {
                case Events.Payment.Created e:
                    this.paymentStorage.Create(e.EntityId.ToIdentifier());
                    break;

                case Events.Payment.AppointmentPaymentInvoiced e:
                    this.paymentStorage.Update(e.EntityId, dto =>
                    {
                        dto.PaymentId = e.EntityId;
                        dto.AppointmentId = e.AppointmentId;
                        dto.Amount = e.Amount;
                        dto.AmountCurrency = e.Currency;
                    });
                    break;

                default:
                    this.logger.LogDebug($"Unknown entity type '{originalEvent.GetType().Name}'");
                    return false;
            }

            return true;
        }
    }
}