using System;

namespace Application.Resources
{
    public class Appointment : IIdentifiableResource
    {
        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }

        public string Id { get; set; }
    }
}