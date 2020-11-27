using ServiceStack;

namespace Api.Interfaces.ServiceOperations
{
    [Route("/clinics", "POST")]
    public class CreateClinicRequest : IReturn<CreateClinicResponse>, IPost
    {
        public int Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
    }
}