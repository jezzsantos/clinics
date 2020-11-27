using System;
using ServiceStack;

namespace Api.Interfaces.ServiceOperations
{
    [Route("/clinics/{Id}/offline", "PUT")]
    public class OfflineDoctorRequest : IReturn<OfflineDoctorResponse>, IPut
    {
        public string Id { get; set; }

        public DateTime FromUtc { get; set; }

        public DateTime ToUtc { get; set; }
    }
}