using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClinicsDomain.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class ClinicOwnerSpec
    {
        [TestMethod]
        public void WhenConstructedWithNullOwnerId_ThenThrows()
        {
            Action a = () => new ClinicOwner(null);

            a.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void WhenConstructed_ThenOwnerIdAssigned()
        {
            var owner = new ClinicOwner("anownerid");

            owner.OwnerId.Should().Be("anownerid");
        }
    }
}