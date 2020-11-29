using System;
using Api.Interfaces.ServiceOperations.Appointments;
using ClinicsApi.Properties;
using ClinicsApi.Services.Appointments;
using Domain.Interfaces.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceStack.FluentValidation;

namespace ClinicsApi.UnitTests.Services.Appointments
{
    [TestClass, TestCategory("Unit")]
    public class CreateAppointmentRequestValidatorSpec
    {
        private CreateAppointmentRequest dto;
        private Mock<IIdentifierFactory> identifierFactory;
        private CreateAppointmentRequestValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            this.identifierFactory = new Mock<IIdentifierFactory>();
            this.identifierFactory.Setup(f => f.IsValid(It.IsAny<Identifier>())).Returns(true);
            this.validator = new CreateAppointmentRequestValidator(this.identifierFactory.Object);
            this.dto = new CreateAppointmentRequest
            {
                StartUtc = DateTime.UtcNow.AddSeconds(1),
                EndUtc = DateTime.UtcNow.AddSeconds(2),
                DoctorId = "adoctorid"
            };
        }

        [TestMethod]
        public void WhenAllProperties_ThenSucceeds()
        {
            this.validator.ValidateAndThrow(this.dto);
        }

        [TestMethod]
        public void WhenStartIsMin_ThenThrows()
        {
            this.dto.StartUtc = DateTime.MinValue;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateAppointmentRequestValidator_StartNotFuture);
        }

        [TestMethod]
        public void WhenStartInPast_ThenThrows()
        {
            this.dto.StartUtc = DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(1));

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateAppointmentRequestValidator_StartNotFuture);
        }

        [TestMethod]
        public void WhenStartIsGreaterThanStartEnd_ThenThrows()
        {
            this.dto.StartUtc = DateTime.UtcNow.AddSeconds(1);
            this.dto.EndUtc = DateTime.UtcNow;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateAppointmentRequestValidator_EndBeforeStart);
        }

        [TestMethod]
        public void WhenStartEndIsMin_ThenThrows()
        {
            this.dto.EndUtc = DateTime.MinValue;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateAppointmentRequestValidator_EndBeforeStart);
        }

        [TestMethod]
        public void WhenStartEndInPast_ThenThrows()
        {
            this.dto.EndUtc = DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(1));

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateAppointmentRequestValidator_EndBeforeStart);
        }

        [TestMethod]
        public void WhenStartEndIsFuture_ThenSucceeds()
        {
            this.dto.EndUtc = DateTime.UtcNow.AddSeconds(1);

            this.validator.ValidateAndThrow(this.dto);
        }
    }
}