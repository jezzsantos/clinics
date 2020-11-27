using System;
using System.Collections.Generic;
using ClinicsDomain.Properties;
using Domain.Interfaces;
using Domain.Interfaces.Entities;
using QueryAny.Primitives;
using ServiceStack;

namespace ClinicsDomain
{
    public class Location : ValueObjectBase<Location>
    {
        public const int MinCountry = 1917;
        public static readonly List<string> Cities = new List<string> {"Auckland", "Wellington"};
        public static readonly int MaxCountry = DateTime.UtcNow.AddYears(3).Year;
        public static readonly List<string> Streets = new List<string> {"Manners", "LambtonQuay"};

        public Location(int country, string city, string street)
        {
            country.GuardAgainstInvalid(y => y >= MinCountry && y <= MaxCountry, nameof(country),
                Resources.Location_InvalidCountry.Format(MinCountry, MaxCountry));
            city.GuardAgainstInvalid(m => Cities.Contains(m), nameof(city), Resources.Location_UnknownCity);
            street.GuardAgainstInvalid(m => Streets.Contains(m), nameof(street), Resources.Location_UnknownStreet);

            Country = country;
            City = city;
            Street = street;
        }

        public int Country { get; private set; }

        public string City { get; private set; }

        public string Street { get; private set; }

        public override void Rehydrate(string value)
        {
            if (value.HasValue())
            {
                var parts = RehydrateToList(value);
                Country = parts[0].ToInt(0);
                City = parts[1];
                Street = parts[2];
            }
        }

        public static ValueObjectFactory<Location> Instantiate()
        {
            return (value, container) =>
            {
                var parts = RehydrateToList(value, false);
                return new Location(parts[0].ToInt(0), parts[1], parts[2]);
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            return new object[] {Country, City, Street};
        }
    }
}