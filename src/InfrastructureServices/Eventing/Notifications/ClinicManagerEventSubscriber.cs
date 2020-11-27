using ApplicationServices;
using ClinicsApplication;
using Domain.Interfaces.Entities;
using InfrastructureServices.Identity;
using PersonsDomain;
using QueryAny.Primitives;

namespace InfrastructureServices.Eventing.Notifications
{
    public class ClinicManagerEventSubscriber : IDomainEventSubscriber
    {
        private readonly IClinicsApplication clinicsApplication;

        public ClinicManagerEventSubscriber(IClinicsApplication clinicsApplication)
        {
            clinicsApplication.GuardAgainstNull(nameof(clinicsApplication));
            this.clinicsApplication = clinicsApplication;
        }

        public bool Notify(IChangeEvent originalEvent)
        {
            switch (originalEvent)
            {
                case Events.Person.EmailChanged e:
                    this.clinicsApplication.UpdatePracticeManagerEmail(new AnonymousCaller(), e.EntityId,
                        e.EmailAddress);
                    return true;

                default:
                    return false;
            }
        }
    }
}