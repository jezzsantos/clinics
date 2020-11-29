using System;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny;

namespace PaymentsDomain
{
    [EntityName("Payment")]
    public class PaymentEntity : AggregateRootBase
    {
        public PaymentEntity(ILogger logger, IIdentifierFactory idFactory) : base(logger, idFactory,
            PaymentsDomain.Events.Payment.Created.Create)
        {
        }

        private PaymentEntity(ILogger logger, IIdentifierFactory idFactory, Identifier identifier)
            : base(logger, idFactory, identifier)
        {
        }

        public AppointmentPayment AppointmentInvoice { get; private set; }

        protected override void OnStateChanged(IChangeEvent @event)
        {
            switch (@event)
            {
                case Events.Payment.Created _:
                    break;

                case Events.Payment.AppointmentPaymentInvoiced added:
                    AppointmentInvoice = new AppointmentPayment(added.AppointmentId.ToIdentifier(), added.Amount,
                        added.Currency);
                    Logger.LogDebug("Payment {Id} added invoice for appointment {Appointment}", Id,
                        added.AppointmentId);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown event {@event.GetType()}");
            }
        }

        public void CreateInvoice(AppointmentPayment appointmentPayment)
        {
            RaiseChangeEvent(PaymentsDomain.Events.Payment.AppointmentPaymentInvoiced.Create(Id, appointmentPayment));
        }

        protected override bool EnsureValidState()
        {
            var isValid = base.EnsureValidState();

            //Additional validation rules that apply to all variations

            return isValid;
        }

        public static AggregateRootFactory<PaymentEntity> Instantiate()
        {
            return (identifier, container, rehydratingProperties) => new PaymentEntity(
                container.Resolve<ILogger>(),
                container.Resolve<IIdentifierFactory>(), identifier);
        }
    }
}