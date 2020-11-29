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

        protected override void OnStateChanged(IChangeEvent @event)
        {
            switch (@event)
            {
                case Events.Payment.Created _:
                    break;

                default:
                    throw new InvalidOperationException($"Unknown event {@event.GetType()}");
            }
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
                container.Resolve<IIdentifierFactory>());
        }
    }
}