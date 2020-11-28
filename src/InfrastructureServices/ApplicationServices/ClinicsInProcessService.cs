using Application.Resources;
using ApplicationServices;
using ClinicsApplication;
using InfrastructureServices.Identity;
using QueryAny.Primitives;

namespace InfrastructureServices.ApplicationServices
{
    public class ClinicsInProcessService : IClinicsService
    {
        private readonly IClinicsApplication clinicsApplication;

        public ClinicsInProcessService(IClinicsApplication clinicsApplication)
        {
            clinicsApplication.GuardAgainstNull(nameof(clinicsApplication));
            this.clinicsApplication = clinicsApplication;
        }

        public Doctor GetDoctor(string id)
        {
            return this.clinicsApplication.GetDoctor(new AnonymousCaller(), id);
        }
    }
}