using System;
using Api.Common.Validators;
using Api.Interfaces.ServiceOperations.Doctors;
using ClinicsApi.Properties;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;
using ServiceStack.FluentValidation;

namespace ClinicsApi.Services.Clinics
{
    internal class OfflineDoctorRequestValidator : AbstractValidator<OfflineDoctorRequest>
    {
        public OfflineDoctorRequestValidator(IIdentifierFactory identifierFactory)
        {
            RuleFor(dto => dto.Id)
                .IsEntityId(identifierFactory)
                .WithMessage(Resources.AnyValidator_InvalidId);

            RuleFor(dto => dto.FromUtc)
                .Must(dto => dto.HasValue())
                .WithMessage(Resources.OfflineDoctorRequestValidator_InvalidFrom);
            When(dto => dto.FromUtc.HasValue(), () =>
            {
                RuleFor(dto => dto.FromUtc)
                    .InclusiveBetween(DateTime.UtcNow, DateTime.MaxValue)
                    .WithMessage(Resources.OfflineDoctorRequestValidator_PastFrom);
                RuleFor(dto => dto.FromUtc)
                    .LessThan(dto => dto.ToUtc)
                    .WithMessage(Resources.OfflineDoctorRequestValidator_FromAfterTo);
            });
            RuleFor(dto => dto.ToUtc)
                .Must(dto => dto.HasValue())
                .WithMessage(Resources.OfflineDoctorRequestValidator_InvalidTo);
            When(dto => dto.ToUtc.HasValue(), () =>
            {
                RuleFor(dto => dto.ToUtc)
                    .InclusiveBetween(DateTime.UtcNow, DateTime.MaxValue)
                    .WithMessage(Resources.OfflineDoctorRequestValidator_PastTo);
            });
        }
    }
}