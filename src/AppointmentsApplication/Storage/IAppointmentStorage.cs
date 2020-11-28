using AppointmentsDomain;
using Domain.Interfaces.Entities;

namespace AppointmentsApplication.Storage
{
    public interface IAppointmentStorage
    {
        AppointmentEntity Load(Identifier id);

        AppointmentEntity Save(AppointmentEntity clinic);
    }
}