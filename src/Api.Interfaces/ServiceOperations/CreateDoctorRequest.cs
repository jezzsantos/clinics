using ServiceStack;

namespace Api.Interfaces.ServiceOperations
{
    [Route("/doctors", "POST")]
    public class CreateDoctorRequest : IReturn<CreateDoctorResponse>, IPost
    {
        public string ClinicId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}