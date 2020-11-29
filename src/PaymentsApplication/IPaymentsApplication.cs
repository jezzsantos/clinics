using Domain.Interfaces;

namespace PaymentsApplication
{
    public interface IPaymentsApplication
    {
        void CreateInvoice(ICurrentCaller caller, string appointmentId, decimal amount, string currency);
    }
}