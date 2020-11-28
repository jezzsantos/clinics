using Api.Common;
using Api.Interfaces.ServiceOperations;
using AppointmentsApplication;
using QueryAny.Primitives;
using ServiceStack;

namespace ClinicsApi.Services.Appointments
{
    public class AppointmentsService : Service
    {
        private readonly IAppointmentsApplication appointmentsApplication;

        public AppointmentsService(IAppointmentsApplication appointmentsApplication)
        {
            appointmentsApplication.GuardAgainstNull(nameof(appointmentsApplication));

            this.appointmentsApplication = appointmentsApplication;
        }

        public CreateAppointmentResponse Post(CreateAppointmentRequest request)
        {
            var appointment =
                this.appointmentsApplication.Create(Request.ToCaller(), request.StartUtc, request.EndUtc,
                    request.DoctorId);
            Response.SetLocation(appointment);
            return new CreateAppointmentResponse
            {
                Appointment = appointment
            };
        }
    }
}