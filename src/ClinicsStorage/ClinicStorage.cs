using System;
using System.Collections.Generic;
using System.Linq;
using ClinicsApplication.ReadModels;
using ClinicsApplication.Storage;
using ClinicsDomain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny;
using QueryAny.Primitives;
using Storage;
using Storage.Interfaces;

namespace ClinicsStorage
{
    public class ClinicStorage : IClinicStorage
    {
        private readonly IEventStreamStorage<ClinicEntity> clinicEventStreamStorage;
        private readonly IQueryStorage<Doctor> doctorQueryStorage;
        private readonly IQueryStorage<Unavailability> unavailabilitiesQueryStorage;

        public ClinicStorage(ILogger logger, IDomainFactory domainFactory,
            IEventStreamStorage<ClinicEntity> eventStreamStorage,
            IRepository repository)
        {
            logger.GuardAgainstNull(nameof(logger));
            domainFactory.GuardAgainstNull(nameof(domainFactory));
            eventStreamStorage.GuardAgainstNull(nameof(eventStreamStorage));
            repository.GuardAgainstNull(nameof(repository));

            this.doctorQueryStorage = new GeneralQueryStorage<Doctor>(logger, domainFactory, repository);
            this.clinicEventStreamStorage = eventStreamStorage;
            this.unavailabilitiesQueryStorage =
                new GeneralQueryStorage<Unavailability>(logger, domainFactory, repository);
        }

        public ClinicStorage(IQueryStorage<Doctor> doctorQueryStorage,
            IEventStreamStorage<ClinicEntity> clinicEventStreamStorage,
            IQueryStorage<Unavailability> unavailabilitiesQueryStorage)
        {
            doctorQueryStorage.GuardAgainstNull(nameof(doctorQueryStorage));
            clinicEventStreamStorage.GuardAgainstNull(nameof(clinicEventStreamStorage));
            unavailabilitiesQueryStorage.GuardAgainstNull(nameof(unavailabilitiesQueryStorage));
            this.doctorQueryStorage = doctorQueryStorage;
            this.clinicEventStreamStorage = clinicEventStreamStorage;
            this.unavailabilitiesQueryStorage = unavailabilitiesQueryStorage;
        }

        public ClinicEntity Load(Identifier id)
        {
            return this.clinicEventStreamStorage.Load(id);
        }

        public ClinicEntity Save(ClinicEntity clinic)
        {
            this.clinicEventStreamStorage.Save(clinic);
            return clinic;
        }

        public List<Doctor> SearchAvailableDoctors(DateTime fromUtc, DateTime toUtc, SearchOptions options)
        {
            var unavailabilities = this.unavailabilitiesQueryStorage.Query(Query.From<Unavailability>()
                    .Where(e => e.From, ConditionOperator.LessThanEqualTo, fromUtc)
                    .AndWhere(e => e.To, ConditionOperator.GreaterThanEqualTo, toUtc))
                .Results;

            var limit = options.Limit;
            var offset = options.Offset;
            options.ClearLimitAndOffset();

            var doctors = this.doctorQueryStorage.Query(Query.From<Doctor>()
                    .WhereAll()
                    .WithSearchOptions(options))
                .Results;

            return doctors
                .Where(clinic => unavailabilities.All(unavailability => unavailability.ClinicId != clinic.Id))
                .Skip(offset)
                .Take(limit)
                .ToList();
        }
    }
}