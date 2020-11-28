using Domain.Interfaces.Entities;
using QueryAny.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicsDomain
{
    public class ClinicDoctor : SingleValueObjectBase<ClinicDoctor, string>
    {
        public ClinicDoctor(string Id) : base(Id)
        {
            Id.GuardAgainstNullOrEmpty(nameof(Id));
        }

        public string Id => Value;

        protected override string ToValue(string value)
        {
            return value;
        }

        public static ValueObjectFactory<ClinicDoctor> Instantiate()
        {
            return (property, container) => new ClinicDoctor(property);
        }
    }
}
