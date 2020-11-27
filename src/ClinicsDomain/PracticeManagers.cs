using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;

namespace ClinicsDomain
{
    public class PracticeManagers : ValueObjectBase<PracticeManagers>
    {
        private List<Identifier> managers;

        public PracticeManagers()
        {
            this.managers = new List<Identifier>();
        }

        public IReadOnlyList<Identifier> Managers => this.managers;

        public void Add(Identifier id)
        {
            id.GuardAgainstNull(nameof(id));

            if (!this.managers.Contains(id))
            {
                this.managers.Add(id);
            }
        }

        public override string Dehydrate()
        {
            return this.managers
                .Select(man => man)
                .Join(";");
        }

        public override void Rehydrate(string value)
        {
            if (value.HasValue())
            {
                this.managers = value.SafeSplit(";")
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
            return new[] {Managers};
        }
    }
}