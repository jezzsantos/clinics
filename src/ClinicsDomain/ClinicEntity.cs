using System;
using ClinicsDomain.Properties;
using Domain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny;

namespace ClinicsDomain
{
    [EntityName("Clinic")]
    public class ClinicEntity : AggregateRootBase
    {
        public ClinicEntity(ILogger logger, IIdentifierFactory idFactory) : base(logger, idFactory,
            ClinicsDomain.Events.Clinic.Created.Create)
        {
        }

        private ClinicEntity(ILogger logger, IIdentifierFactory idFactory, Identifier identifier) : base(logger,
            idFactory,
            identifier)
        {
        }

        public Location Location { get; private set; }

        public ClinicOwner Owner { get; private set; }

        public PracticeManagers Managers { get; private set; }

        public ClinicLicense License { get; private set; }

        public Unavailabilities Unavailabilities { get; } = new Unavailabilities();

        protected override void OnStateChanged(IChangeEvent @event)
        {
            switch (@event)
            {
                case Events.Clinic.Created _:
                    break;

                case Events.Clinic.LocationChanged changed:
                    Location = new Location(changed.Country, changed.City, changed.Street);
                    Logger.LogDebug("Clinic {Id} changed location to {Country}, {City}, {Street}", Id, changed.Country,
                        changed.City, changed.Street);
                    break;

                case Events.Clinic.OwnershipChanged changed:
                    Owner = new ClinicOwner(changed.Owner);
                    Managers = new PracticeManagers();
                    Managers.Add(changed.Owner.ToIdentifier());

                    Logger.LogDebug("Clinic {Id} changed ownership to {Owner}", Id, Owner);
                    break;

                case Events.Clinic.RegistrationChanged changed:
                    License = new ClinicLicense(changed.Jurisdiction, changed.Number);

                    Logger.LogDebug("Clinic {Id} registration changed to {Jurisdiction}, {CertificateNumber}", Id,
                        changed.Jurisdiction, changed.Number);
                    break;

                case Events.Clinic.DoctorUnavailabilitySlotAdded added:
                    var unavailability = new UnavailabilityEntity(Logger, IdFactory);
                    added.EntityId = unavailability.Id;
                    unavailability.SetAggregateEventHandler(RaiseChangeEvent);
                    RaiseToEntity(unavailability, @event);
                    Unavailabilities.Add(unavailability);
                    Logger.LogDebug("Doctor {Id} had been made unavailable from {From} until {To}", Id, added.From,
                        added.To);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown event {@event.GetType()}");
            }
        }

        public void SetLocation(Location location)
        {
            RaiseChangeEvent(ClinicsDomain.Events.Clinic.LocationChanged.Create(Id, location));
        }

        public void SetOwnership(ClinicOwner owner)
        {
            RaiseChangeEvent(ClinicsDomain.Events.Clinic.OwnershipChanged.Create(Id, owner));
        }

        public void Register(ClinicLicense plate)
        {
            RaiseChangeEvent(ClinicsDomain.Events.Clinic.RegistrationChanged.Create(Id, plate));
        }

        public void OfflineDoctor(TimeSlot slot)
        {
            RaiseChangeEvent(ClinicsDomain.Events.Clinic.DoctorUnavailabilitySlotAdded.Create(Id, slot,
                UnavailabilityCausedBy.Offline, null));
        }

        public void AddDoctorUnavailability(TimeSlot slot, UnavailabilityCausedBy causedBy, string causedByReference)
        {
            RaiseChangeEvent(
                ClinicsDomain.Events.Clinic.DoctorUnavailabilitySlotAdded.Create(Id, slot, causedBy,
                    causedByReference));
        }

        protected override bool EnsureValidState()
        {
            var isValid = base.EnsureValidState();

            Unavailabilities.EnsureValidState();

            if (Unavailabilities.Count > 0)
            {
                if (!Location.HasValue())
                {
                    throw new RuleViolationException(Resources.ClinicEntity_NotLocated);
                }
                if (!Owner.HasValue())
                {
                    throw new RuleViolationException(Resources.ClinicEntity_NotOwned);
                }
                if (!License.HasValue())
                {
                    throw new RuleViolationException(Resources.ClinicEntity_NotRegistered);
                }
            }

            return isValid;
        }

        public static AggregateRootFactory<ClinicEntity> Instantiate()
        {
            return (identifier, container, rehydratingProperties) => new ClinicEntity(container.Resolve<ILogger>(),
                container.Resolve<IIdentifierFactory>(), identifier);
        }
    }
}