using System;
using Domain.Interfaces.Entities;

namespace PaymentsDomain
{
    public static class Events
    {
        public static class Payment
        {
            public class Created : IChangeEvent
            {
                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static Created Create(Identifier id)
                {
                    return new Created
                    {
                        EntityId = id,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class AppointmentPaymentInvoiced : IChangeEvent
            {
                public string Currency { get; set; }

                public decimal Amount { get; set; }

                public string AppointmentId { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static AppointmentPaymentInvoiced Create(Identifier id, AppointmentPayment payment)
                {
                    return new AppointmentPaymentInvoiced
                    {
                        EntityId = id,
                        AppointmentId = payment.AppointmentId,
                        Amount = payment.Amount,
                        Currency = payment.Currency,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }
        }
    }
}