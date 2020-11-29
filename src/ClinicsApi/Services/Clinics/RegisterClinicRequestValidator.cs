using Api.Common.Validators;
using Api.Interfaces.ServiceOperations.Clinics;
using ClinicsApi.Properties;
using ClinicsDomain;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;
using ServiceStack.FluentValidation;

namespace ClinicsApi.Services.Clinics
{
    internal class RegisterClinicRequestValidator : AbstractValidator<RegisterClinicRequest>
    {
        public RegisterClinicRequestValidator(IIdentifierFactory identifierFactory)
        {
            RuleFor(dto => dto.Id)
                .IsEntityId(identifierFactory)
                .WithMessage(Resources.AnyValidator_InvalidId);
            RuleFor(dto => dto.Jurisdiction)
                .NotEmpty();
            When(dto => dto.Jurisdiction.HasValue(), () =>
            {
                RuleFor(dto => dto.Jurisdiction)
                    .Matches(Validations.Clinic.Jurisdiction.Expression)
                    .Must(dto => ClinicLicense.Jurisdictions.Contains(dto))
                    .WithMessage(Resources.RegisterClinicRequestValidator_InvalidJurisdiction);
            });
            RuleFor(dto => dto.CertificateNumber)
                .NotEmpty();
            When(dto => dto.CertificateNumber.HasValue(), () =>
            {
                RuleFor(dto => dto.CertificateNumber)
                    .Matches(Validations.Clinic.Number.Expression)
                    .WithMessage(Resources.RegisterClinicRequestValidator_InvalidNumber);
            });
        }
    }
}