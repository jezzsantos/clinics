using PaymentsDomain;

namespace PaymentsApplication.Storage
{
    public interface IPaymentStorage
    {
        PaymentEntity Save(PaymentEntity payment);
    }
}