using System;
using Api.Common.Validators;
using Api.Interfaces.ServiceOperations;
using ClinicsApi.Properties;
using Domain.Interfaces.Entities;
using ServiceStack.FluentValidation;

namespace ClinicsApi.Services.Appointments
{
    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator(IIdentifierFactory identifierFactory)
        {
            RuleFor(dto => dto.StartUtc).Must(date => date > DateTime.UtcNow)
                .WithMessage(Resources.CreateAppointmentRequestValidator_StartNotFuture);
            RuleFor(dto => dto).Must(date => date.EndUtc > date.StartUtc)
                .WithMessage(Resources.CreateAppointmentRequestValidator_EndBeforeStart);
            RuleFor(dto => dto.DoctorId).IsEntityId(identifierFactory)
                .WithMessage(Resources.AnyValidator_InvalidId);
        }
    }
}