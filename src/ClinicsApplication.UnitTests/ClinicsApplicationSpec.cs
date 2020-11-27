using System;
using System.Collections.Generic;
using System.Linq;
using Application.Resources;
using ApplicationServices;
using ClinicsApplication.Storage;
using ClinicsDomain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ClinicLicense = ClinicsDomain.ClinicLicense;
using ClinicOwner = ClinicsDomain.ClinicOwner;
using Doctor = ClinicsApplication.ReadModels.Doctor;

namespace ClinicsApplication.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class ClinicsApplicationSpec
    {
        private Mock<ICurrentCaller> caller;
        private ClinicsApplication clinicsApplication;
        private Mock<IIdentifierFactory> idFactory;
        private Mock<ILogger> logger;
        private Mock<IPersonsService> personService;
        private Mock<IClinicStorage> storage;

        [TestInitialize]
        public void Initialize()
        {
            this.logger = new Mock<ILogger>();
            this.idFactory = new Mock<IIdentifierFactory>();
            this.idFactory.Setup(idf => idf.Create(It.IsAny<IIdentifiableEntity>()))
                .Returns("anid".ToIdentifier());
            this.idFactory.Setup(idf => idf.IsValid(It.IsAny<Identifier>()))
                .Returns(true);
            this.storage = new Mock<IClinicStorage>();
            this.personService = new Mock<IPersonsService>();
            this.caller = new Mock<ICurrentCaller>();
            this.caller.Setup(c => c.Id).Returns("acallerid");
            this.clinicsApplication = new ClinicsApplication(this.logger.Object, this.idFactory.Object,
                this.storage.Object,
                this.personService.Object);
        }

        [TestMethod]
        public void WhenCreate_ThenReturnsClinic()
        {
            var person = new Person
                {Id = "apersonid"};
            this.personService.Setup(ps => ps.Get(It.IsAny<string>()))
                .Returns(person);
            var city = Location.Cities[0];
            var street = Location.Streets[0];
            var entity = new ClinicEntity(this.logger.Object, this.idFactory.Object);
            this.storage.Setup(s =>
                    s.Save(It.IsAny<ClinicEntity>()))
                .Returns(entity);

            var result = this.clinicsApplication.Create(this.caller.Object, 2010, city, street);

            result.Id.Should().Be("anid");
            this.storage.Verify(s =>
                s.Save(It.Is<ClinicEntity>(e =>
                    e.Owner == "apersonid"
                    && e.Location.Country == 2010
                    && e.Location.City == city
                    && e.Location.Street == street
                    && e.Managers.Managers.Single() == "apersonid")));
        }

        [TestMethod]
        public void WhenRegister_ThenRegistersClinic()
        {
            var entity = new ClinicEntity(this.logger.Object, this.idFactory.Object);
            this.storage.Setup(s => s.Load(It.Is<Identifier>(i => i == "aclinicid")))
                .Returns(entity);
            this.storage.Setup(s =>
                    s.Save(It.Is<ClinicEntity>(
                        e => e.License == new ClinicLicense(ClinicLicense.Jurisdictions[0], "anumber"))))
                .Returns(entity);

            var result =
                this.clinicsApplication.Register(this.caller.Object, "aclinicid", ClinicLicense.Jurisdictions[0],
                    "anumber");

            result.License.Should().BeEquivalentTo(new Application.Resources.ClinicLicense
                {Jurisdiction = ClinicLicense.Jurisdictions[0], CertificateNumber = "anumber"});
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenReserve_ThenReservesClinic()
        {
            var fromUtc = DateTime.UtcNow.AddMinutes(1);
            var toUtc = fromUtc.AddMinutes(1);
            var entity = new ClinicEntity(this.logger.Object, this.idFactory.Object);
            entity.SetLocation(new Location(2010, Location.Cities[0], Location.Streets[0]));
            entity.SetOwnership(new ClinicOwner("anownerid"));
            entity.Register(new ClinicLicense(ClinicLicense.Jurisdictions[0], "anumber"));
            this.storage.Setup(s => s.Load(It.Is<Identifier>(i => i == "aclinicid")))
                .Returns(entity);
            this.storage.Setup(s => s.Save(It.Is<ClinicEntity>(e => e.Unavailabilities.Count == 1)))
                .Returns(entity);

            var result = this.clinicsApplication.OfflineDoctor(this.caller.Object, "aclinicid", fromUtc, toUtc);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public void WhenSearchAvailableDoctors_ThenReturnsAvailableDoctors()
        {
            this.storage.Setup(s =>
                    s.SearchAvailableDoctors(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<SearchOptions>()))
                .Returns(new List<Doctor>
                    {new Doctor()});

            var result =
                this.clinicsApplication.SearchAvailableDoctors(this.caller.Object, DateTime.MinValue, DateTime.MinValue,
                    new SearchOptions(), new GetOptions());

            result.Results.Count.Should().Be(1);
        }
    }
}