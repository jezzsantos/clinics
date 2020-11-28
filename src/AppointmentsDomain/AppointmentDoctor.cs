using Domain.Interfaces.Entities;

namespace AppointmentsDomain
{
    public class AppointmentDoctor : SingleValueObjectBase<AppointmentDoctor, string>
    {
        public AppointmentDoctor(string doctorId) : base(doctorId)
        {
        }

        public string DoctorId => Value;

        protected override string ToValue(string value)
        {
            return value;
        }

        public static ValueObjectFactory<AppointmentDoctor> Instantiate()
        {
            return (property, container) => new AppointmentDoctor(property);
        }
    }
}