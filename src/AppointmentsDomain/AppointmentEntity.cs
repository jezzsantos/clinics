﻿using System;
using AppointmentsDomain.Properties;
using Domain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny;
using QueryAny.Primitives;

namespace AppointmentsDomain
{
    [EntityName("Appointment")]
    public class AppointmentEntity : AggregateRootBase
    {
        public AppointmentEntity(ILogger logger, IIdentifierFactory idFactory) :
            base(logger, idFactory, AppointmentsDomain.Events.Appointment.Created.Create)
        {
        }

        private AppointmentEntity(ILogger logger, IIdentifierFactory idFactory, Identifier identifier)
            : base(logger, idFactory, identifier)
        {
        }

        public DateTime StartUtc { get; private set; }

        public DateTime EndUtc { get; private set; }

        public string DoctorId { get; private set; }

        public void SetDetails(AppointmentDoctor doctor, TimeSlot slot)
        {
            RaiseChangeEvent(
                AppointmentsDomain.Events.Appointment.DetailsChanged.Create(Id, doctor, slot));
        }

        protected override void OnStateChanged(IChangeEvent @event)
        {
            switch (@event)
            {
                case Events.Appointment.Created _:
                    break;

                case Events.Appointment.DetailsChanged changed:
                    DoctorId = changed.DoctorId;
                    StartUtc = changed.StartUtc;
                    EndUtc = changed.EndUtc;

                    Logger.LogDebug("Appointment {Id} changed details for {Doctor}", Id, DoctorId);
                    break;

                default:
                    throw new InvalidOperationException($"Unknown event {@event.GetType()}");
            }
        }

        protected override bool EnsureValidState()
        {
            var isValid = base.EnsureValidState();

            if (StartUtc.HasValue())
            {
                if (!StartUtc.IsFutureDate())
                {
                    throw new RuleViolationException(Resources.AppointmentEntity_StartIsNotFuture);
                }
                if (EndUtc.IsBeforeOrEqual(StartUtc))
                {
                    throw new RuleViolationException(Resources.AppointmentEntity_EndBeforeStart);
                }
            }

            return isValid;
        }

        public static AggregateRootFactory<AppointmentEntity> Instantiate()
        {
            return (identifier, container, rehydratingProperties) => new AppointmentEntity(
                container.Resolve<ILogger>(),
                container.Resolve<IIdentifierFactory>(), identifier);
        }
    }
}