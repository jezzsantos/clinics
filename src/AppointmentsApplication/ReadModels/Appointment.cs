using System;
using QueryAny;
using Storage.Interfaces.ReadModels;

namespace AppointmentsApplication.ReadModels
{
    [EntityName("Appointment")]
    public class Appointment : IReadModelEntity
    {
        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }

        public string Id { get; set; }
    }
}