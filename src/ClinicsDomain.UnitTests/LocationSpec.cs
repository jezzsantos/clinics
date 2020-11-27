using System;
using ClinicsDomain.Properties;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClinicsDomain.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class LocationSpec
    {
        [TestMethod]
        public void WhenConstructAndZeroCountry_ThenThrows()
        {
            Action a = () => new Location(0, Location.Cities[0], Location.Streets[0]);
            a.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessageLike(Resources.Location_InvalidCountry);
        }

        [TestMethod]
        public void WhenConstructAndCountryLessThanMin_ThenThrows()
        {
            Action a = () => new Location(Location.MinCountry - 1, Location.Cities[0], Location.Streets[0]);
            a.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessageLike(Resources.Location_InvalidCountry);
        }

        [TestMethod]
        public void WhenConstructAndCountryGreaterThanMax_ThenThrows()
        {
            Action a = () => new Location(Location.MaxCountry + 1, Location.Cities[0], Location.Streets[0]);
            a.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessageLike(Resources.Location_InvalidCountry);
        }

        [TestMethod]
        public void WhenConstructAndCityUnknown_ThenThrows()
        {
            Action a = () => new Location(Location.MinCountry, "unknown", Location.Streets[0]);
            a.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessageLike(Resources.Location_UnknownCity);
        }

        [TestMethod]
        public void WhenConstructAndStreetUnknown_ThenThrows()
        {
            Action a = () => new Location(Location.MinCountry, Location.Cities[0], "unknown");
            a.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessageLike(Resources.Location_UnknownStreet);
        }

        [TestMethod]
        public void WhenConstruct_ThenSucceeds()
        {
            var manufacturer = new Location(Location.MinCountry, Location.Cities[0], Location.Streets[0]);

            manufacturer.Country.Should().Be(Location.MinCountry);
        }
    }
}