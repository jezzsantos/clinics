using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Doctors
{
    [Route("/doctors", "POST")]
    public class RegisterDoctorRequest : PostOperation<RegisterDoctorResponse>
    {
        public string ClinicId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}