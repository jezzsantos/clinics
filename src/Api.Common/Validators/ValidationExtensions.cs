using System.Text.RegularExpressions;
using Api.Common.Properties;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Validators;

namespace Api.Common.Validators
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> Matches<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, ValidationFormat format)
        {
            return ruleBuilder.SetValidator(new ValidatorFormatValidator(format));
        }

        public static IRuleBuilderOptions<T, TProperty> IsEmail<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new EmailValidator());
        }

        public static IRuleBuilderOptions<TDto, string> IsEntityId<TDto>(this IRuleBuilderInitial<TDto, string> rule,
            IIdentifierFactory identifierFactory)
        {
            return rule.Must(id => id.HasValue() && identifierFactory.IsValid(Identifier.Create(id)));
        }
    }

    internal class ValidatorFormatValidator : PropertyValidator
    {
        private readonly ValidationFormat format;

        public ValidatorFormatValidator(ValidationFormat format)
            : base(Resources.ValidationFormatValidator_InvalidFormat)
        {
            this.format = format;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return false;
            }

            var propertyValue = context.PropertyValue.ToString();
            if (!propertyValue.HasValue())
            {
                return false;
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return Regex.IsMatch(propertyValue, this.format.Expression);
        }
    }

    internal class EmailValidator : PropertyValidator
    {
        public EmailValidator()
            : base(Resources.EmailValidator_InvalidEmail)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return false;
            }

            var propertyValue = context.PropertyValue?.ToString();
            if (!propertyValue.HasValue())
            {
                return false;
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return Regex.IsMatch(propertyValue, Validations.Email.Expression);
        }
    }
}