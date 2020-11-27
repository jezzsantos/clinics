using System.Collections.Generic;
using ClinicsDomain.Properties;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;

namespace ClinicsDomain
{
    public class ClinicLicense : ValueObjectBase<ClinicLicense>
    {
        public static readonly List<string> Jurisdictions = new List<string> {"New Zealand", "Australia"};

        public ClinicLicense(string jurisdiction, string certificateNumber)
        {
            jurisdiction.GuardAgainstNullOrEmpty(nameof(certificateNumber));
            certificateNumber.GuardAgainstNullOrEmpty(nameof(certificateNumber));
            jurisdiction.GuardAgainstInvalid(val => Jurisdictions.Contains(val), nameof(jurisdiction),
                Resources.ClinicLicense_UnknownJurisdiction);
            certificateNumber.GuardAgainstInvalid(Validations.Clinic.Number, nameof(certificateNumber));
            Jurisdiction = jurisdiction;
            CertificateNumber = certificateNumber;
        }

        public string Jurisdiction { get; private set; }

        public string CertificateNumber { get; private set; }

        public override void Rehydrate(string value)
        {
            var parts = RehydrateToList(value);
            Jurisdiction = parts[0];
            CertificateNumber = parts[1];
        }

        public static ValueObjectFactory<ClinicLicense> Instantiate()
        {
            return (property, container) =>
            {
                var parts = RehydrateToList(property, false);
                return new ClinicLicense(parts[0], parts[1]);
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new[] {Jurisdiction, CertificateNumber};
        }
    }
}