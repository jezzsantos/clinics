using Api.Common.Validators;
using Api.Interfaces.ServiceOperations;
using ClinicsApi.Properties;
using ClinicsDomain;
using Domain.Interfaces.Entities;
using ServiceStack.FluentValidation;

namespace ClinicsApi.Services.Clinics
{
    internal class CreateDoctorRequestValidator : AbstractValidator<CreateDoctorRequest>
    {
        public CreateDoctorRequestValidator(IIdentifierFactory identifierFactory)
        {
            RuleFor(dto => dto.ClinicId)
                .IsEntityId(identifierFactory)
                .WithMessage(
                    Resources.AnyValidator_InvalidId);
            RuleFor(dto => dto.FirstName)
                .NotEmpty()
                .Matches(Validations.Doctor.FirstName)
                .WithMessage(Resources.CreateDoctorRequestValidator_InvalidFirstName);
            RuleFor(dto => dto.LastName)
                .NotEmpty()
                .Matches(Validations.Doctor.LastName)
                .WithMessage(Resources.CreateDoctorRequestValidator_InvalidLastName);
        }
    }
}