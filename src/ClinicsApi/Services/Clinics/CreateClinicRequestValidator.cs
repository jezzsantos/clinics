using Api.Interfaces.ServiceOperations.Clinics;
using ClinicsApi.Properties;
using ClinicsDomain;
using QueryAny.Primitives;
using ServiceStack.FluentValidation;

namespace ClinicsApi.Services.Clinics
{
    internal class CreateClinicRequestValidator : AbstractValidator<CreateClinicRequest>
    {
        public CreateClinicRequestValidator()
        {
            RuleFor(dto => dto.Country)
                .InclusiveBetween(Location.MinCountry, Location.MaxCountry)
                .WithMessage(
                    Resources.CreateClinicRequestValidator_InvalidCountry.Format(Location.MinCountry,
                        Location.MaxCountry));
            RuleFor(dto => dto.City)
                .NotEmpty();
            When(dto => dto.City.HasValue(), () =>
            {
                RuleFor(dto => dto.City)
                    .Must(val => Location.Cities.Contains(val))
                    .WithMessage(
                        Resources.CreateClinicRequestValidator_InvalidCity.Format(string.Join(", ", Location.Cities)));
            });
            RuleFor(dto => dto.Street)
                .NotEmpty();
            When(dto => dto.Street.HasValue(), () =>
            {
                RuleFor(dto => dto.Street)
                    .NotEmpty()
                    .Must(val => Location.Streets.Contains(val))
                    .WithMessage(
                        Resources.CreateClinicRequestValidator_InvalidStreet.Format(string.Join(", ",
                            Location.Streets)));
            });
        }
    }
}