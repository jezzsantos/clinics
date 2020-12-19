using Domain.Interfaces;
using Domain.Interfaces.Entities;
using DomainServices;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonsDomain.Properties;

namespace PersonsDomain.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class PersonEntitySpec
    {
        private PersonAggregate aggregate;
        private Mock<IIdentifierFactory> identifierFactory;
        private Mock<ILogger> logger;
        private Mock<IEmailService> uniqueEmailService;

        [TestInitialize]
        public void Initialize()
        {
            this.logger = new Mock<ILogger>();
            this.identifierFactory = new Mock<IIdentifierFactory>();
            this.identifierFactory.Setup(f => f.Create(It.IsAny<IIdentifiableEntity>()))
                .Returns("apersonid".ToIdentifier);
            this.uniqueEmailService = new Mock<IEmailService>();
            this.uniqueEmailService.Setup(ues => ues.EnsureEmailIsUnique(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            this.aggregate = new PersonAggregate(this.logger.Object, this.identifierFactory.Object,
                this.uniqueEmailService.Object);
        }

        [TestMethod]
        public void WhenSetName_ThenNameAndDisplayNameAssigned()
        {
            this.aggregate.SetName(new PersonName("afirstname", "alastname"));

            this.aggregate.Name.Should().Be(new PersonName("afirstname", "alastname"));
            this.aggregate.DisplayName.Should().Be(new PersonDisplayName("afirstname"));
            this.aggregate.Events[1].Should().BeOfType<Events.Person.NameChanged>();
        }

        [TestMethod]
        public void WhenSetDisplayName_ThenEmailAssigned()
        {
            this.aggregate.SetDisplayName(new PersonDisplayName("adisplayname"));

            this.aggregate.DisplayName.Should().Be(new PersonDisplayName("adisplayname"));
            this.aggregate.Events[1].Should().BeOfType<Events.Person.DisplayNameChanged>();
        }

        [TestMethod]
        public void WhenSetEmail_ThenEmailAssigned()
        {
            this.aggregate.SetEmail(new Email("anemail@company.com"));

            this.aggregate.Email.Should().Be(new Email("anemail@company.com"));
            this.aggregate.Events[1].Should().BeOfType<Events.Person.EmailChanged>();
        }

        [TestMethod]
        public void WhenSetEmailAndNotUnique_ThenThrows()
        {
            this.uniqueEmailService.Setup(ues => ues.EnsureEmailIsUnique(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            this.aggregate
                .Invoking(x =>
                    x.SetEmail(new Email("anemail@company.com")))
                .Should().Throw<RuleViolationException>(Resources.PersonEntity_EmailNotUnique);
        }

        [TestMethod]
        public void WhenSetPhoneNumber_ThenEmailAssigned()
        {
            this.aggregate.SetPhoneNumber(new PhoneNumber("+64277888111"));

            this.aggregate.Phone.Should().Be(new PhoneNumber("+64277888111"));
            this.aggregate.Events[1].Should().BeOfType<Events.Person.PhoneNumberChanged>();
        }
    }
}