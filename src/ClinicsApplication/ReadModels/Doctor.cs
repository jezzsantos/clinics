using QueryAny;
using Storage.Interfaces;
using Storage.Interfaces.ReadModels;

namespace ClinicsApplication.ReadModels
{
    [EntityName("Doctor")]
    public class Doctor : IReadModelEntity, IHasIdentity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ClinicId { get; set; }

        public string Id { get; set; }
    }
}