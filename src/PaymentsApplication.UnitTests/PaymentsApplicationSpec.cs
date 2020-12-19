using Domain.Interfaces;
using Domain.Interfaces.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentsApplication.Storage;
using PaymentsDomain;

namespace PaymentsApplication.UnitTests
{
    [TestClass, TestCategory("Unit")]
    public class PaymentsApplicationSpec
    {
        private Mock<ICurrentCaller> caller;
        private Mock<IIdentifierFactory> idFactory;
        private Mock<ILogger> logger;
        private PaymentsApplication paymentsApplication;
        private Mock<IPaymentStorage> storage;

        [TestInitialize]
        public void Initialize()
        {
            this.logger = new Mock<ILogger>();
            this.idFactory = new Mock<IIdentifierFactory>();
            this.idFactory.Setup(idf => idf.Create(It.IsAny<IIdentifiableEntity>()))
                .Returns("anid".ToIdentifier());
            this.idFactory.Setup(idf => idf.IsValid(It.IsAny<Identifier>()))
                .Returns(true);
            this.storage = new Mock<IPaymentStorage>();
            this.caller = new Mock<ICurrentCaller>();
            this.caller.Setup(c => c.Id).Returns("acallerid");
            this.paymentsApplication = new PaymentsApplication(this.logger.Object, this.idFactory.Object,
                this.storage.Object);
        }

        [TestMethod]
        public void WhenCreateInvoice_ThenReturnsPayment()
        {
            var entity = new PaymentEntity(this.logger.Object, this.idFactory.Object);
            this.storage.Setup(s =>
                    s.Save(It.IsAny<PaymentEntity>()))
                .Returns(entity);

            this.paymentsApplication.CreateInvoice(this.caller.Object, "anappointmentid", 25M, "acurrency");

            this.storage.Verify(s =>
                s.Save(It.Is<PaymentEntity>(e =>
                    e.AppointmentInvoice.AppointmentId == "anappointmentid"
                    && e.AppointmentInvoice.Amount == 25M
                    && e.AppointmentInvoice.Currency == "acurrency")));
        }
    }
}