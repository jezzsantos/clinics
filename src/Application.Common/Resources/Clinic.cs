using System.Collections.Generic;

namespace Application.Resources
{
    public class Clinic : IIdentifiableResource
    {
        public ClinicAddress Address { get; set; }

        public ClinicLicense License { get; set; }

        public ClinicOwner Owner { get; set; }

        public List<ClinicPracticeManager> PracticeManagers { get; set; }

        public string Id { get; set; }
    }

    public class ClinicPracticeManager
    {
        public string Id { get; set; }
    }

    public class ClinicOwner
    {
        public string Id { get; set; }
    }

    public class ClinicLicense
    {
        public string Jurisdiction { get; set; }

        public string CertificateNumber { get; set; }
    }

    public class ClinicAddress
    {
        public int Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
    }
}