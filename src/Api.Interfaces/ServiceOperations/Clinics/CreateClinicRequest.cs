using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Clinics
{
    [Route("/clinics", "POST")]
    public class CreateClinicRequest : PostOperation<CreateClinicResponse>
    {
        public int Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
    }
}