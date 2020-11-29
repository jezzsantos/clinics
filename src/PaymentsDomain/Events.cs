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
        }
    }
}