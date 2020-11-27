using Api.Interfaces.ServiceOperations;
using ClinicsApi.Properties;
using ClinicsApi.Services.Clinics;
using ClinicsDomain;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.FluentValidation;

namespace ClinicsApi.UnitTests.Services.Clinics
{
    [TestClass, TestCategory("Unit")]
    public class CreateClinicRequestValidatorSpec
    {
        private CreateClinicRequest dto;
        private CreateClinicRequestValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            this.validator = new CreateClinicRequestValidator();
            this.dto = new CreateClinicRequest
            {
                Country = Location.MinCountry,
                City = Location.Cities[0],
                Street = Location.Streets[0]
            };
        }

        [TestMethod]
        public void WhenAllProperties_ThenSucceeds()
        {
            this.validator.ValidateAndThrow(this.dto);
        }

        [TestMethod]
        public void WhenCountryIsZero_ThenThrows()
        {
            this.dto.Country = 0;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateClinicRequestValidator_InvalidCountry);
        }

        [TestMethod]
        public void WhenCountryIsTooOld_ThenThrows()
        {
            this.dto.Country = Location.MinCountry - 1;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateClinicRequestValidator_InvalidCountry);
        }

        [TestMethod]
        public void WhenCountryIsTooNew_ThenThrows()
        {
            this.dto.Country = Location.MaxCountry + 1;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateClinicRequestValidator_InvalidCountry);
        }

        [TestMethod]
        public void WhenCityIsNull_ThenThrows()
        {
            this.dto.City = null;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageForNotEmpty();
        }

        [TestMethod]
        public void WhenCityIsUnknown_ThenThrows()
        {
            this.dto.City = "unknowncity";

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateClinicRequestValidator_InvalidCity);
        }

        [TestMethod]
        public void WhenStreetIsNull_ThenThrows()
        {
            this.dto.Street = null;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageForNotEmpty();
        }

        [TestMethod]
        public void WhenStreetIsUnknown_ThenThrows()
        {
            this.dto.Street = "unknownstreet";

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.CreateClinicRequestValidator_InvalidStreet);
        }
    }
}