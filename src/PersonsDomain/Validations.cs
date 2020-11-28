using Domain.Interfaces;
using PhoneNumbers;
using QueryAny.Primitives;

namespace PersonsDomain
{
    public static class Validations
    {
        public static class Person
        {
            public static readonly ValidationFormat Email =
                new ValidationFormat(
                    @"^(?:[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+\.)*[\w\!\#\$\%\&\'\*\+\-\/\=\?\^\`\{\|\}\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\-](?!\.)){0,61}[a-zA-Z0-9]?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\[(?:(?:[01]?\d{1,2}|2[0-4]\d|25[0-5])\.){3}(?:[01]?\d{1,2}|2[0-4]\d|25[0-5])\]))$");

            public static readonly ValidationFormat Name = Domain.Interfaces.Validations.DescriptiveName();

            public static readonly ValidationFormat PhoneNumber = new ValidationFormat(value =>
            {
                if (!value.HasValue())
                {
                    return false;
                }

                if (!value.StartsWith("+"))
                {
                    return false;
                }

                var util = PhoneNumberUtil.GetInstance();
                try
                {
                    var number = util.Parse(value, null);
                    return util.IsValidNumber(number);
                }
                catch (NumberParseException)
                {
                    return false;
                }
            });
        }
    }
}