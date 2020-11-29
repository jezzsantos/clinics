using System;
using Domain;
using Domain.Interfaces.Entities;

namespace AppointmentsDomain
{
    public static class Events
    {
        public static class Appointment
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

            public class DetailsChanged : IChangeEvent
            {
                public string DoctorId { get; set; }

                public DateTime EndUtc { get; set; }

                public DateTime StartUtc { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static DetailsChanged Create(Identifier id, AppointmentDoctor doctor, TimeSlot slot)
                {
                    return new DetailsChanged
                    {
                        EntityId = id,
                        StartUtc = slot.From,
                        EndUtc = slot.To,
                        DoctorId = doctor.DoctorId,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }

            public class AppointmentStarted : IChangeEvent
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

            public class AppointmentEnded : IChangeEvent
            {
                public decimal CostAmount { get; set; }

                public string CostCurrency { get; set; }

                public string EntityId { get; set; }

                public DateTime ModifiedUtc { get; set; }

                public static AppointmentEnded Create(Identifier id, decimal amount, string currency)
                {
                    return new AppointmentEnded
                    {
                        EntityId = id,
                        CostAmount = amount,
                        CostCurrency = currency,
                        ModifiedUtc = DateTime.UtcNow
                    };
                }
            }
        }
    }
}