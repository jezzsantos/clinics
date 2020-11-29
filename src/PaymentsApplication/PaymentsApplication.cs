using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using PaymentsApplication.Storage;
using PaymentsDomain;
using QueryAny.Primitives;

namespace PaymentsApplication
{
    public class PaymentsApplication : IPaymentsApplication
    {
        private readonly IIdentifierFactory idFactory;
        private readonly ILogger logger;
        private readonly IPaymentStorage storage;

        public PaymentsApplication(ILogger logger, IIdentifierFactory idFactory, IPaymentStorage storage)
        {
            logger.GuardAgainstNull(nameof(logger));
            idFactory.GuardAgainstNull(nameof(idFactory));
            storage.GuardAgainstNull(nameof(storage));

            this.logger = logger;
            this.idFactory = idFactory;
            this.storage = storage;
        }

        public void CreateInvoice(ICurrentCaller caller, string appointmentId, decimal amount, string currency)
        {
            caller.GuardAgainstNull(nameof(caller));

            var payment = new PaymentEntity(this.logger, this.idFactory);
            payment.CreateInvoice(new AppointmentPayment(appointmentId.ToIdentifier(), amount, currency));

            var created = this.storage.Save(payment);

            this.logger.LogInformation("Payment {Id} was created for appointment {Appointment} by {Caller}", created.Id,
                appointmentId, caller.Id);
        }
    }
}