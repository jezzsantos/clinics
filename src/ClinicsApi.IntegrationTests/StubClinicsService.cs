using Application.Resources;
using ApplicationServices;

namespace ClinicsApi.IntegrationTests
{
    public class StubClinicsService : IClinicsService
    {
        public Doctor GetDoctor(string id)
        {
            return new Doctor
            {
                Id = id,
                Name = new PersonName
                {
                    FirstName = "afirstname",
                    LastName = "alastname"
                }
            };
        }
    }
}