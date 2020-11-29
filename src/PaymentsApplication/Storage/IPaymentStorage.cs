using Domain.Interfaces.Entities;
using PaymentsDomain;

namespace PaymentsApplication.Storage
{
    public interface IPaymentStorage
    {
        PaymentEntity Load(Identifier id);

        PaymentEntity Save(PaymentEntity payment);
    }
}