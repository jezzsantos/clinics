using System;
using ServiceStack;

namespace Api.Interfaces.ServiceOperations
{
    [Route("/appointments", "POST")]
    public class CreateAppointmentRequest : IReturn<CreateAppointmentResponse>, IPost
    {
        public string DoctorId { get; set; }

        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }
    }
}