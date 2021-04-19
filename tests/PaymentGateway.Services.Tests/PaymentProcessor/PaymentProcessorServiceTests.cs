using Moq;
using NUnit.Framework;
using PaymentGateway.Services.Banking.Interface;
using PaymentGateway.Services.Storage.Interface;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.PaymentProcessor;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Common.Models.Enums;
using System;
using FluentAssertions;

namespace PaymentGateway.Services.Tests.PaymentProcessor
{
    public partial class PaymentProcessorServiceTests
    {
        private protected Mock<IBankingService> _mockedBankingService;
        private protected Mock<IStorageService<PaymentAudit>> _mockedStorageService;
        private protected PaymentProcessorService _sut;

        [SetUp]
        public void SetUp()
        {
            _mockedBankingService = new Mock<IBankingService>();
            _mockedStorageService = new Mock<IStorageService<PaymentAudit>>();
            _sut = new PaymentProcessorService(_mockedBankingService.Object, _mockedStorageService.Object);
        }

        [Test]
        public void GivenIBankingServiceIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new PaymentProcessorService(null, _mockedStorageService.Object));
            output.ParamName.Should().Be("bankingService");
        }

        [Test]
        public void GivenIStorageServiceIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new PaymentProcessorService(_mockedBankingService.Object,
             null));
            output.ParamName.Should().Be("storageService");
        }

        private PaymentProcessingRequest GetPaymentProcessingRequest()
        {
            return new PaymentProcessingRequest()
            {
                CardNumber = "000000-0000",
                CVV = 123,
                CurrencyCode = "ABC",
                ExpiryMonth = 12,
                ExpiryYear = 2010,
                Amount = 123
            };
        }

        private PaymentAudit GetPaymentAudit(Status status)
        {
            return new PaymentAudit()
            {
                CardNumber = GetPaymentProcessingRequest().CardNumber,
                CurrencyCode = GetPaymentProcessingRequest().CurrencyCode,
                ExpiryMonth = GetPaymentProcessingRequest().ExpiryMonth,
                ExpiryYear = GetPaymentProcessingRequest().ExpiryYear,
                CVV = GetPaymentProcessingRequest().CVV,
                Amount = GetPaymentProcessingRequest().Amount,
                Status = status
            };
        }
    }
}