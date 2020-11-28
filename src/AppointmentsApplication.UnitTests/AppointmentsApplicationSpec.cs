using System;
using Application.Resources;
using ApplicationServices;
using AppointmentsApplication.Storage;
using AppointmentsDomain;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AppointmentsApplication.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class AppointmentsApplicationSpec
    {
        private AppointmentsApplication appointmentsApplication;
        private Mock<ICurrentCaller> caller;
        private Mock<IClinicsService> clinicsService;
        private Mock<IIdentifierFactory> idFactory;
        private Mock<ILogger> logger;
        private Mock<IAppointmentStorage> storage;

        [TestInitialize]
        public void Initialize()
        {
            this.logger = new Mock<ILogger>();
            this.idFactory = new Mock<IIdentifierFactory>();
            this.idFactory.Setup(idf => idf.Create(It.IsAny<IIdentifiableEntity>()))
                .Returns("anid".ToIdentifier());
            this.idFactory.Setup(idf => idf.IsValid(It.IsAny<Identifier>()))
                .Returns(true);
            this.storage = new Mock<IAppointmentStorage>();
            this.clinicsService = new Mock<IClinicsService>();
            this.caller = new Mock<ICurrentCaller>();
            this.caller.Setup(c => c.Id).Returns("acallerid");
            this.appointmentsApplication = new AppointmentsApplication(this.logger.Object, this.idFactory.Object,
                this.storage.Object, this.clinicsService.Object);
        }

        [TestMethod]
        public void WhenCreate_ThenReturnsClinic()
        {
            var start = DateTime.UtcNow.AddDays(1);
            var end = start.AddDays(1);
            this.clinicsService.Setup(cs => cs.GetDoctor(It.IsAny<string>()))
                .Returns(new Doctor {Id = "adoctorid"});
            var entity = new AppointmentEntity(this.logger.Object, this.idFactory.Object);
            this.storage.Setup(s =>
                    s.Save(It.IsAny<AppointmentEntity>()))
                .Returns(entity);

            var result = this.appointmentsApplication.Create(this.caller.Object, start, end, null);

            result.Id.Should().Be("anid");
            this.storage.Verify(s =>
                s.Save(It.Is<AppointmentEntity>(e =>
                    e.StartUtc == start
                    && e.EndUtc == end
                    && e.DoctorId == null)));
        }
    }
}