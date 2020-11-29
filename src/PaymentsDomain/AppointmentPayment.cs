using System.Collections.Generic;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;
using ServiceStack;

namespace PaymentsDomain
{
    public class AppointmentPayment : ValueObjectBase<AppointmentPayment>
    {
        public AppointmentPayment(Identifier appointmentId, decimal amount, string currency)
        {
            appointmentId.GuardAgainstNull(nameof(appointmentId));
            currency.GuardAgainstNullOrEmpty(nameof(appointmentId));

            AppointmentId = appointmentId;
            Amount = amount;
            Currency = currency;
        }

        public Identifier AppointmentId { get; private set; }

        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public override void Rehydrate(string value)
        {
            var parts = RehydrateToList(value, false);

            AppointmentId = parts[0].ToIdentifier();
            Amount = parts[1].ToDecimal();
            Currency = parts[2];
        }

        public static ValueObjectFactory<AppointmentPayment> Instantiate()
        {
            return (property, container) =>
            {
                var parts = RehydrateToList(property, false);
                return new AppointmentPayment(parts[0].ToIdentifier(), parts[1].ToDecimal(), parts[2]);
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new object[] {AppointmentId, Amount, Currency};
        }
    }
}