using Api.Interfaces.ServiceOperations.Doctors;
using ClinicsApi.Properties;
using ClinicsApi.Services.Clinics;
using Domain.Interfaces.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceStack.FluentValidation;

namespace ClinicsApi.UnitTests.Services.Clinics
{
    [TestClass, TestCategory("Unit")]
    public class RegisterDoctorRequestValidatorSpec
    {
        private RegisterDoctorRequest dto;
        private Mock<IIdentifierFactory> identifierFactory;
        private RegisterDoctorRequestValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            this.identifierFactory = new Mock<IIdentifierFactory>();
            this.identifierFactory.Setup(f => f.IsValid(It.IsAny<Identifier>())).Returns(true);
            this.validator = new RegisterDoctorRequestValidator(this.identifierFactory.Object);
            this.dto = new RegisterDoctorRequest
            {
                ClinicId = "aclinicid",
                FirstName = "afirstname",
                LastName = "alastname"
            };
        }

        [TestMethod]
        public void WhenAllProperties_ThenSucceeds()
        {
            this.validator.ValidateAndThrow(this.dto);
        }

        [TestMethod]
        public void WhenFirstNameIsNull_ThenThrows()
        {
            this.dto.FirstName = null;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateDoctorRequestValidator_InvalidFirstName);
        }

        [TestMethod]
        public void WhenFirstNameIsInvalid_ThenThrows()
        {
            this.dto.FirstName = "^invalid";

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateDoctorRequestValidator_InvalidFirstName);
        }

        [TestMethod]
        public void WhenLastNameIsNull_ThenThrows()
        {
            this.dto.LastName = null;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateDoctorRequestValidator_InvalidLastName);
        }

        [TestMethod]
        public void WhenLastNameIsInvalid_ThenThrows()
        {
            this.dto.LastName = "^invalid";

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateDoctorRequestValidator_InvalidLastName);
        }
    }
}