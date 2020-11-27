using System;
using Api.Common.Validators;
using Api.Interfaces.ServiceOperations;
using ClinicsApi.Properties;
using QueryAny.Primitives;
using ServiceStack.FluentValidation;

namespace ClinicsApi.Services.Clinics
{
    internal class SearchAvailableDoctorsRequestValidator : AbstractValidator<SearchAvailableDoctorsRequest>
    {
        public SearchAvailableDoctorsRequestValidator(IHasSearchOptionsValidator hasSearchOptionsValidator)
        {
            hasSearchOptionsValidator.GuardAgainstNull(nameof(hasSearchOptionsValidator));

            Include(hasSearchOptionsValidator);

            When(dto => dto.FromUtc.HasValue, () =>
            {
                RuleFor(dto => dto.FromUtc)
                    .Must(dto => dto.GetValueOrDefault().HasValue())
                    .WithMessage(Resources.SearchAvailableDoctorsRequestValidator_InvalidFrom);
                RuleFor(dto => dto.FromUtc)
                    .InclusiveBetween(DateTime.UtcNow, DateTime.MaxValue)
                    .WithMessage(Resources.SearchAvailableDoctorsRequestValidator_PastFrom);
                RuleFor(dto => dto.FromUtc)
                    .LessThan(dto => dto.ToUtc)
                    .WithMessage(Resources.SearchAvailableDoctorsRequestValidator_FromAfterTo);
            });
            When(dto => dto.ToUtc.HasValue, () =>
            {
                RuleFor(dto => dto.ToUtc)
                    .Must(dto => dto.GetValueOrDefault().HasValue())
                    .WithMessage(Resources.SearchAvailableDoctorsRequestValidator_InvalidTo);
                RuleFor(dto => dto.ToUtc)
                    .InclusiveBetween(DateTime.UtcNow, DateTime.MaxValue)
                    .WithMessage(Resources.SearchAvailableDoctorsRequestValidator_PastTo);
            });
        }
    }
}