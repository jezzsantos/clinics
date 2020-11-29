using System;
using Application.Resources;
using Domain.Interfaces;

namespace AppointmentsApplication
{
    public interface IAppointmentsApplication
    {
        Appointment Create(ICurrentCaller caller, in DateTime startUtc, in DateTime endUtc, string doctorId);

        Appointment End(ICurrentCaller caller, string id);
    }
}