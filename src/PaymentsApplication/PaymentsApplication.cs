using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using PaymentsApplication.Storage;
using QueryAny.Primitives;

namespace PaymentsApplication
{
    public class PaymentsApplication : IPaymentsApplication
    {
        private readonly ILogger logger;
        private readonly IPaymentStorage storage;

        public PaymentsApplication(ILogger logger, IPaymentStorage storage)
        {
            logger.GuardAgainstNull(nameof(logger));
            storage.GuardAgainstNull(nameof(storage));

            this.logger = logger;
            this.storage = storage;
        }

        public void CreateInvoice(ICurrentCaller caller, string appointmentId, decimal amount, string currency)
        {
            caller.GuardAgainstNull(nameof(caller));

            //TODO: create the invoice
        }
    }
}