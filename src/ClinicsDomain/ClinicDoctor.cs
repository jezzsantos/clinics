using System.Collections.Generic;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;

namespace ClinicsDomain
{
    public class ClinicDoctor : ValueObjectBase<ClinicDoctor>
    {
        public ClinicDoctor(string id, string firstName, string lastName)
        {
            id.GuardAgainstNullOrEmpty(nameof(id));
            firstName.GuardAgainstNullOrEmpty(nameof(firstName));
            lastName.GuardAgainstNullOrEmpty(nameof(lastName));
            firstName.GuardAgainstInvalid(Validations.Doctor.FirstName, nameof(firstName));
            lastName.GuardAgainstInvalid(Validations.Doctor.LastName, nameof(lastName));

            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Id { get; private set; }

        public override void Rehydrate(string value)
        {
            var parts = RehydrateToList(value);
            Id = parts[0];
            FirstName = parts[1];
            LastName = parts[2];
        }

        public static ValueObjectFactory<ClinicDoctor> Instantiate()
        {
            return (property, container) =>
            {
                var parts = RehydrateToList(property, false);
                return new ClinicDoctor(parts[0], parts[1], parts[2]);
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new[] {Id, FirstName, LastName};
        }
    }
}