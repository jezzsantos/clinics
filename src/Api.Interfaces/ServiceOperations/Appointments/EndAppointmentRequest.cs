using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Appointments
{
    [Route("/appointments/{Id}/end", "PATCH")]
    public class EndAppointmentRequest : PatchOperation<EndAppointmentResponse>
    {
        public string Id { get; set; }
    }
}