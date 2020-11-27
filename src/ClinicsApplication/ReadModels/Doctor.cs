using QueryAny;
using Storage.Interfaces.ReadModels;

namespace ClinicsApplication.ReadModels
{
    [EntityName("Doctor")]
    public class Doctor : IReadModelEntity
    {
        public string Name { get; set; }

        public string Id { get; set; }
    }
}