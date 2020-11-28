using Domain.Interfaces.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClinicsDomain.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class ClinicDoctorsSpec
    {
        private ClinicDoctors managers;

        [TestInitialize]
        public void Initialize()
        {
            this.managers = new ClinicDoctors();
        }

        [TestMethod]
        public void WhenConstructed_ThenHasNoDoctors()
        {
            this.managers.Doctors.Should().BeEmpty();
        }

        [TestMethod]
        public void WhenAddAndDoctorNotExist_ThenAddsDoctor()
        {
            this.managers.Add("amanagerid".ToIdentifier());

            this.managers.Doctors.Count.Should().Be(1);
            this.managers.Doctors[0].Should().Be("amanagerid".ToIdentifier());
        }

        [TestMethod]
        public void WhenAddAndDoctorAndExist_ThenOnlySingleDoctor()
        {
            this.managers.Add("amanagerid".ToIdentifier());
            this.managers.Add("amanagerid".ToIdentifier());
            this.managers.Add("amanagerid".ToIdentifier());

            this.managers.Doctors.Count.Should().Be(1);
            this.managers.Doctors[0].Should().Be("amanagerid".ToIdentifier());
        }
    }
}