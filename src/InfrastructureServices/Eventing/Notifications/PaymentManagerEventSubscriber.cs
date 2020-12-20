using ApplicationServices;
using Domain.Interfaces.Entities;
using InfrastructureServices.Identity;
using PaymentsApplication;
using QueryAny.Primitives;

namespace InfrastructureServices.Eventing.Notifications
{
    public class PaymentManagerEventSubscriber : IDomainEventSubscriber
    {
        private readonly IPaymentsApplication paymentsApplication;

        public PaymentManagerEventSubscriber(IPaymentsApplication paymentsApplication)
        {
            paymentsApplication.GuardAgainstNull(nameof(paymentsApplication));
            this.paymentsApplication = paymentsApplication;
        }

        public bool Notify(IChangeEvent originalEvent)
        {
            switch (originalEvent)
            {
                case AppointmentsDomain.Events.Appointment.AppointmentEnded e:
                    this.paymentsApplication.CreateInvoice(new AnonymousCaller(), e.EntityId, e.CostAmount,
                        e.CostCurrency);
                    return true;

                default:
                    return false;
            }
        }
    }
}