using Api.Interfaces.ServiceOperations.Appointments;
using ClinicsApi.Services.Appointments;
using Domain.Interfaces.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceStack.FluentValidation;

namespace ClinicsApi.UnitTests.Services.Appointments
{
    [TestClass, TestCategory("Unit")]
    public class EndAppointmentRequestValidatorSpec
    {
        private EndAppointmentRequest dto;
        private Mock<IIdentifierFactory> identifierFactory;
        private EndAppointmentRequestValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            this.identifierFactory = new Mock<IIdentifierFactory>();
            this.identifierFactory.Setup(f => f.IsValid(It.IsAny<Identifier>())).Returns(true);
            this.validator = new EndAppointmentRequestValidator(this.identifierFactory.Object);
            this.dto = new EndAppointmentRequest
            {
                Id = "anid"
            };
        }

        [TestMethod]
        public void WhenAllProperties_ThenSucceeds()
        {
            this.validator.ValidateAndThrow(this.dto);
        }
    }
}