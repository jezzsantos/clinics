using Domain.Interfaces.Entities;
using QueryAny.Primitives;

namespace ClinicsDomain
{
    public class ClinicOwner : SingleValueObjectBase<ClinicOwner, string>
    {
        public ClinicOwner(string ownerId) : base(ownerId)
        {
            ownerId.GuardAgainstNullOrEmpty(nameof(ownerId));
        }

        public string OwnerId => Value;

        protected override string ToValue(string value)
        {
            return value;
        }

        public static ValueObjectFactory<ClinicOwner> Instantiate()
        {
            return (property, container) => new ClinicOwner(property);
        }
    }
}