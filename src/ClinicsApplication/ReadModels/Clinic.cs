using System.Collections.Generic;
using QueryAny;
using Storage.Interfaces.ReadModels;

namespace ClinicsApplication.ReadModels
{
    [EntityName("Clinic")]
    public class Clinic : IReadModelEntity
    {
        public int LocationCountry { get; set; }

        public string LocationCity { get; set; }

        public string LocationStreet { get; set; }

        public string ClinicOwnerId { get; set; }

        public List<string> PracticeManagerIds { get; set; }

        public string LicenseJurisdiction { get; set; }

        public string LicenseCertificateNumber { get; set; }

        public string Id { get; set; }
    }
}