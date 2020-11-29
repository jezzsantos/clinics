using System;
using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Appointments
{
    [Route("/appointments", "POST")]
    public class CreateAppointmentRequest : PostOperation<CreateAppointmentResponse>
    {
        public string DoctorId { get; set; }

        public DateTime StartUtc { get; set; }

        public DateTime EndUtc { get; set; }
    }
}