using Moq;
using PaymentGateway.Services.Banking.Interface;
using NUnit.Framework;
using PaymentGateway.Services.Banking;
using System;
using FluentAssertions;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.Banking.Models;

namespace PaymentGateway.Services.Tests.Banking
{
    public partial class BankingServiceTests
    {
        private protected Mock<IBankingRefitServiceProvider> _mockedBankingService;
        private protected BankingService _sut;

        [SetUp]
        public void Setup()
        {
            _mockedBankingService = new Mock<IBankingRefitServiceProvider>();
            _sut = new BankingService(_mockedBankingService.Object);
        }

        [Test]
        public void GivenBankingRestServiceIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new BankingService(null));

            output.ParamName.Should().Be("bankingService");
        }

        private PaymentProcessingRequest GetPaymentProcessingRequest()
        {
            return new PaymentProcessingRequest()
            {
                CurrencyCode = "GBP",
                ExpiryMonth = 1,
                ExpiryYear = 12,
                CVV = 123,
                CardNumber = "9123-819203-8091",
                Amount = 123.00
            };
        }

        private BankingResponse GetBankingResponse()
        {
            return new BankingResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Successful = true
            };
        }
        
    }
}