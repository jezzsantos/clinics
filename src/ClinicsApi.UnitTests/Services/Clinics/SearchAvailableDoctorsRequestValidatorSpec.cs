﻿using System;
using Api.Common.Validators;
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
    public class SearchAvailableDoctorsRequestValidatorSpec
    {
        private SearchAvailableDoctorsRequest dto;
        private Mock<IIdentifierFactory> identifierFactory;
        private SearchAvailableDoctorsRequestValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            this.identifierFactory = new Mock<IIdentifierFactory>();
            this.identifierFactory.Setup(f => f.IsValid(It.IsAny<Identifier>())).Returns(true);
            this.validator =
                new SearchAvailableDoctorsRequestValidator(new HasSearchOptionsValidator(new HasGetOptionsValidator()));
            this.dto = new SearchAvailableDoctorsRequest
            {
                FromUtc = null,
                ToUtc = null
            };
        }

        [TestMethod]
        public void WhenAllProperties_ThenSucceeds()
        {
            this.validator.ValidateAndThrow(this.dto);
        }

        [TestMethod]
        public void WhenFromIsMin_ThenThrows()
        {
            this.dto.FromUtc = DateTime.MinValue;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.SearchAvailableDoctorsRequestValidator_InvalidFrom);
        }

        [TestMethod]
        public void WhenFromInPast_ThenThrows()
        {
            this.dto.FromUtc = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1));

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.SearchAvailableDoctorsRequestValidator_PastFrom);
        }

        [TestMethod]
        public void WhenFromIsGreaterThanTo_ThenThrows()
        {
            this.dto.FromUtc = DateTime.UtcNow.AddSeconds(1);
            this.dto.ToUtc = DateTime.UtcNow;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.SearchAvailableDoctorsRequestValidator_FromAfterTo);
        }

        [TestMethod]
        public void WhenToIsMin_ThenThrows()
        {
            this.dto.ToUtc = DateTime.MinValue;

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.SearchAvailableDoctorsRequestValidator_InvalidTo);
        }

        [TestMethod]
        public void WhenToInPast_ThenThrows()
        {
            this.dto.ToUtc = DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1));

            this.validator
                .Invoking(x => x.ValidateAndThrow(this.dto))
                .Should().Throw<ValidationException>()
                .WithValidationMessageLike(Resources.SearchAvailableDoctorsRequestValidator_PastTo);
        }

        [TestMethod]
        public void WhenToIsFuture_ThenSucceeds()
        {
            this.dto.ToUtc = DateTime.UtcNow.AddSeconds(1);

            this.validator.ValidateAndThrow(this.dto);
        }
    }
}