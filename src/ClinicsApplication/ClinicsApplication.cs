using System;
using System.Collections.Generic;
using System.Linq;
using Application;
using Application.Resources;
using ApplicationServices;
using ClinicsApplication.Storage;
using ClinicsDomain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny.Primitives;
using ServiceStack;
using ClinicLicense = ClinicsDomain.ClinicLicense;
using ClinicOwner = Application.Resources.ClinicOwner;

namespace ClinicsApplication
{
    public class ClinicsApplication : ApplicationBase, IClinicsApplication
    {
        private readonly IIdentifierFactory idFactory;
        private readonly ILogger logger;
        private readonly IPersonsService personsService;
        private readonly IClinicStorage storage;

        public ClinicsApplication(ILogger logger, IIdentifierFactory idFactory, IClinicStorage storage,
            IPersonsService personsService)
        {
            logger.GuardAgainstNull(nameof(logger));
            idFactory.GuardAgainstNull(nameof(idFactory));
            storage.GuardAgainstNull(nameof(storage));
            personsService.GuardAgainstNull(nameof(personsService));
            this.logger = logger;
            this.idFactory = idFactory;
            this.storage = storage;
            this.personsService = personsService;
        }

        public Clinic Create(ICurrentCaller caller, int country, string city, string street)
        {
            caller.GuardAgainstNull(nameof(caller));

            var owner = this.personsService.Get(caller.Id)
                .ToClinicOwner();

            var clinic = new ClinicEntity(this.logger, this.idFactory);
            clinic.SetOwnership(new ClinicsDomain.ClinicOwner(owner.Id));
            clinic.SetLocation(new Location(country, city, street));

            var created = this.storage.Save(clinic);

            this.logger.LogInformation("Clinic {Id} was created by {Caller}", created.Id, caller.Id);

            return created.ToClinic();
        }

        public Clinic Register(ICurrentCaller caller, string id, string jurisdiction, string certificateNumber)
        {
            caller.GuardAgainstNull(nameof(caller));
            id.GuardAgainstNullOrEmpty(nameof(id));

            var clinic = this.storage.Load(id.ToIdentifier());
            if (id == null)
            {
                throw new ResourceNotFoundException();
            }

            var license = new ClinicLicense(jurisdiction, certificateNumber);
            clinic.Register(license);
            var updated = this.storage.Save(clinic);

            this.logger.LogInformation("Clinic {Id} was registered with plate {License}, by {Caller}", id, license,
                caller.Id);

            return updated.ToClinic();
        }

        public Clinic OfflineDoctor(ICurrentCaller caller, string id, DateTime fromUtc, DateTime toUtc)
        {
            caller.GuardAgainstNull(nameof(caller));
            id.GuardAgainstNullOrEmpty(nameof(id));
            fromUtc.GuardAgainstMinValue(nameof(fromUtc));
            toUtc.GuardAgainstMinValue(nameof(toUtc));

            var clinic = this.storage.Load(id.ToIdentifier());
            if (id == null)
            {
                throw new ResourceNotFoundException();
            }

            clinic.OfflineDoctor(new TimeSlot(fromUtc, toUtc));
            var updated = this.storage.Save(clinic);

            this.logger.LogInformation("Doctor {Id} was taken offline from {From} until {To}, by {Caller}",
                id, fromUtc, toUtc, caller.Id);

            return updated.ToClinic();
        }

        public SearchResults<Doctor> SearchAvailableDoctors(ICurrentCaller caller, DateTime fromUtc, DateTime toUtc,
            SearchOptions searchOptions,
            GetOptions getOptions)
        {
            caller.GuardAgainstNull(nameof(caller));

            var doctors = this.storage.SearchAvailableDoctors(fromUtc, toUtc, searchOptions);

            this.logger.LogInformation("Available doctors were retrieved by {Caller}", caller.Id);

            return searchOptions.ApplyWithMetadata(doctors
                .ConvertAll(doc => WithGetOptions(doc.ToDoctor(), getOptions)));
        }

        public void UpdatePracticeManagerEmail(ICurrentCaller caller, string managerId, string email)
        {
            caller.GuardAgainstNull(nameof(caller));

            //TODO: find the clinics that have this manager, and update them, then raise email notification
        }

        // ReSharper disable once UnusedParameter.Local

        private static Doctor WithGetOptions(Doctor doctor, GetOptions options)
        {
            // TODO: expand embedded resources, etc
            return doctor;
        }
    }

    public static class ClinicConversionExtensions
    {
        public static Doctor ToDoctor(this ReadModels.Doctor readModel)
        {
            var dto = readModel.ConvertTo<Doctor>();
            return dto;
        }

        public static Clinic ToClinic(this ClinicEntity entity)
        {
            var dto = entity.ConvertTo<Clinic>();
            dto.Id = entity.Id;
            dto.Address = entity.Location.ConvertTo<ClinicAddress>();
            dto.Owner = entity.Owner.ToClinicOwner();
            dto.PracticeManagers = entity.Managers.ToPracticeManagers();

            return dto;
        }

        private static List<ClinicPracticeManager> ToPracticeManagers(this PracticeManagers managers)
        {
            return managers.HasValue()
                ? new List<ClinicPracticeManager>(managers.Managers.Select(id => new ClinicPracticeManager {Id = id}))
                : new List<ClinicPracticeManager>();
        }

        private static ClinicOwner ToClinicOwner(this ClinicsDomain.ClinicOwner owner)
        {
            return owner.HasValue()
                ? new ClinicOwner {Id = owner}
                : null;
        }

        public static ClinicOwner ToClinicOwner(this Person person)
        {
            var owner = person.ConvertTo<ClinicOwner>();

            return owner;
        }
    }
}