using System;
using ClinicsDomain.Properties;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClinicsDomain.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class ClinicLicenseSpec
    {
        [TestMethod]
        public void WhenConstructAndNullJurisdiction_ThenThrows()
        {
            Action a = () => new ClinicLicense(null, "anumber");
            a.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenConstructAndUnknownJurisdiction_ThenThrows()
        {
            Action a = () => new ClinicLicense("unknown", "anumber");
            a.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessageLike(Resources.ClinicLicense_UnknownJurisdiction);
        }

        [TestMethod]
        public void WhenConstructAndNullNumber_ThenThrows()
        {
            Action a = () => new ClinicLicense(ClinicLicense.Jurisdictions[0], null);
            a.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenConstructAndInvalidNumber_ThenThrows()
        {
            Action a = () => new ClinicLicense(ClinicLicense.Jurisdictions[0], "^invalid^");
            a.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void WhenConstructAndKnownJurisdiction_ThenSucceeds()
        {
            var plate = new ClinicLicense(ClinicLicense.Jurisdictions[0], "anumber");

            plate.Jurisdiction.Should().Be(ClinicLicense.Jurisdictions[0]);
            plate.CertificateNumber.Should().Be("anumber");
        }
    }
}