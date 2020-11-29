using ServiceStack;

namespace Api.Interfaces.ServiceOperations.Clinics
{
    [Route("/clinics/{Id}/register", "PUT")]
    public class RegisterClinicRequest : PutOperation<RegisterClinicResponse>
    {
        public string Id { get; set; }

        public string Jurisdiction { get; set; }

        public string CertificateNumber { get; set; }
    }
}