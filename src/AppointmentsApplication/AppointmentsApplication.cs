using System;
using Application.Resources;
using ApplicationServices;
using AppointmentsApplication.Storage;
using AppointmentsDomain;
using Domain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using QueryAny.Primitives;
using ServiceStack;

namespace AppointmentsApplication
{
    public class AppointmentsApplication : IAppointmentsApplication
    {
        private readonly IClinicsService clinicsService;
        private readonly IIdentifierFactory idFactory;
        private readonly ILogger logger;
        private readonly IAppointmentStorage storage;

        public AppointmentsApplication(ILogger logger, IIdentifierFactory idFactory, IAppointmentStorage storage,
            IClinicsService clinicsService)
        {
            logger.GuardAgainstNull(nameof(logger));
            idFactory.GuardAgainstNull(nameof(idFactory));
            storage.GuardAgainstNull(nameof(storage));
            clinicsService.GuardAgainstNull(nameof(clinicsService));

            this.logger = logger;
            this.idFactory = idFactory;
            this.storage = storage;
            this.clinicsService = clinicsService;
        }

        public Appointment Create(ICurrentCaller caller, in DateTime startUtc, in DateTime endUtc, string doctorId)
        {
            caller.GuardAgainstNull(nameof(caller));

            var doctor = this.clinicsService.GetDoctor(doctorId)
                .ToAppointmentDoctor();

            var clinic = new AppointmentEntity(this.logger, this.idFactory);
            var slot = new TimeSlot(startUtc, endUtc);
            clinic.SetDetails(doctor, slot);

            var created = this.storage.Save(clinic);

            this.logger.LogInformation("Appointment {Id} was created by {Caller}", created.Id, caller.Id);

            return created.ToAppointment();
        }
    }

    public static class AppointmentConversionExtensions
    {
        public static AppointmentDoctor ToAppointmentDoctor(this Doctor doctor)
        {
            return doctor.ConvertTo<AppointmentDoctor>();
        }

        public static Appointment ToAppointment(this AppointmentEntity entity)
        {
            var dto = entity.ConvertTo<Appointment>();
            dto.Id = entity.Id;

            return dto;
        }
    }
}