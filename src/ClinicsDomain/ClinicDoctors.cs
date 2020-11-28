using System.Collections.Generic;
using System.Linq;
using ClinicsDomain.Properties;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;

namespace ClinicsDomain
{
    public class ClinicDoctors : ValueObjectBase<ClinicDoctors>
    {
        private List<Identifier> doctors;

        public ClinicDoctors()
        {
            this.doctors = new List<Identifier>();
        }

        public IReadOnlyList<Identifier> Doctors => this.doctors;

        public void Add(Identifier id)
        {
            id.GuardAgainstNull(nameof(id));

            if (!this.doctors.Contains(id))
            {
                this.doctors.Add(id);
            }
        }

        public override string Dehydrate()
        {
            return this.doctors
                .Select(man => man)
                .Join(";");
        }

        public override void Rehydrate(string value)
        {
            if (value.HasValue())
            {
                this.doctors = value.SafeSplit(";")
                    .Select(Identifier.Create)
                    .ToList();
            }
        }

        public static ValueObjectFactory<PracticeManagers> Instantiate()
        {
            return (value, container) => new PracticeManagers();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new[] {Doctors};
        }

        public void EnsureValidState()
        {
            if (HasDuplicates())
            {
                throw new RuleViolationException(Resources.ClinicDoctors_DuplicateDoctors);
            }
        }

        private bool HasDuplicates()
        {
            var doctorsSet = new HashSet<Identifier>(Doctors);
            return Doctors.Count > doctorsSet.Count();
        }
    }
}