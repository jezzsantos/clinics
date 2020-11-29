using System;
using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Doctors
{
    [Route("/clinics/{Id}/offline", "PUT")]
    public class OfflineDoctorRequest : PutOperation<OfflineDoctorResponse>
    {
        public string Id { get; set; }

        public DateTime FromUtc { get; set; }

        public DateTime ToUtc { get; set; }
    }
}