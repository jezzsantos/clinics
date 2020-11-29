using QueryAny;
using Storage.Interfaces.ReadModels;

namespace PaymentsApplication.ReadModels
{
    [EntityName("Payment")]
    public class Payment : IReadModelEntity
    {
        public string PaymentId { get; set; }

        public string AppointmentId { get; set; }

        public decimal Amount { get; set; }

        public string AmountCurrency { get; set; }

        public string Id { get; set; }
    }
}