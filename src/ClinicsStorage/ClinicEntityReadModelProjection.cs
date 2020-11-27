using System;
using System.Collections.Generic;
using ClinicsApplication.ReadModels;
using ClinicsDomain;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny.Primitives;
using Storage;
using Storage.Interfaces.ReadModels;

namespace ClinicsStorage
{
    public class ClinicEntityReadModelProjection : IReadModelProjection
    {
        private readonly IReadModelStorage<Clinic> clinicStorage;
        private readonly ILogger logger;
        private readonly IReadModelStorage<Unavailability> unavailabilityStorage;

        public ClinicEntityReadModelProjection(ILogger logger, IRepository repository)
        {
            logger.GuardAgainstNull(nameof(logger));
            repository.GuardAgainstNull(nameof(repository));

            this.logger = logger;
            this.clinicStorage = new GeneralReadModelStorage<Clinic>(logger, repository);
            this.unavailabilityStorage = new GeneralReadModelStorage<Unavailability>(logger, repository);
        }

        public Type EntityType => typeof(ClinicEntity);

        public bool Project(IChangeEvent originalEvent)
        {
            switch (originalEvent)
            {
                case Events.Clinic.Created e:
                    this.clinicStorage.Create(e.EntityId.ToIdentifier());
                    break;

                case Events.Clinic.LocationChanged e:
                    this.clinicStorage.Update(e.EntityId, dto =>
                    {
                        dto.LocationCountry = e.Country;
                        dto.LocationCity = e.City;
                        dto.LocationStreet = e.Street;
                    });
                    break;

                case Events.Clinic.OwnershipChanged e:
                    this.clinicStorage.Update(e.EntityId, dto =>
                    {
                        dto.ClinicOwnerId = e.Owner;
                        dto.PracticeManagerIds = new List<string> {e.Owner};
                    });
                    break;

                case Events.Clinic.RegistrationChanged e:
                    this.clinicStorage.Update(e.EntityId, dto =>
                    {
                        dto.LicenseJurisdiction = e.Jurisdiction;
                        dto.LicenseCertificateNumber = e.Number;
                    });
                    break;

                case Events.Clinic.DoctorUnavailabilitySlotAdded e:
                    this.unavailabilityStorage.Create(e.EntityId, dto =>
                    {
                        dto.ClinicId = e.ClinicId;
                        dto.From = e.From;
                        dto.To = e.To;
                        dto.CausedBy = e.CausedBy;
                        dto.CausedByReference = e.CausedByReference;
                    });
                    break;

                default:
                    this.logger.LogDebug($"Unknown entity type '{originalEvent.GetType().Name}'");
                    return false;
            }

            return true;
        }
    }
}