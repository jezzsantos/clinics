using Domain.Interfaces;

namespace ClinicsDomain
{
    public static class Validations
    {
        public static class Clinic
        {
            public static readonly ValidationFormat Jurisdiction = new ValidationFormat(@"^[\d\w\-\. ]{1,50}$", 1, 50);
            public static readonly ValidationFormat Number = new ValidationFormat(@"^[\d\w ]{1,15}$", 1, 15);
        }

        public static class Doctor
        {
            public static readonly ValidationFormat FirstName = Domain.Interfaces.Validations.DescriptiveName(1, 50);
            public static readonly ValidationFormat LastName = Domain.Interfaces.Validations.DescriptiveName(1, 50);
        }
    }
}