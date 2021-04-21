using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.Banking.Exceptions;

namespace PaymentGateway.Services.Tests.Banking
{
    public partial class BankingServiceTests
    {
        [Test]
        public void GivenRequestIsNull_WhenICallProcessPayment_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.ThrowsAsync<ArgumentNullException>(()=> _sut.ProcessPayment(null));

            output.ParamName.Should().Be("paymentRequest");
        }

        [Test]
        public void GivenValidRequest_WhenICallProcessPayment_ThenAValidBankingResponseIsReturned()
        {
            var expectedResponse = GetBankingResponse();
            _mockedBankingService.Setup(x=> x.ProcessPayment(It.IsAny<PaymentProcessingRequest>()))
            .ReturnsAsync(expectedResponse);

            var output = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result;

            output.Successful.Should().Be(expectedResponse.Successful);
        }

        [Ignore("Because I was not able to host the bank api in the same docker compose I had to mock the reponse expected from banking api")]
        public void GivenRestProviderThrowsAnException_WhenICallProcessPayment_ThenBankingServiceExceptionIsThrown()
        {
            var genException = new Exception("fake-error");
            _mockedBankingService.Setup(x=> x.ProcessPayment(It.IsAny<PaymentProcessingRequest>()))
            .ThrowsAsync(genException);

            var output = Assert.ThrowsAsync<BankingServiceException>(()=> _sut.ProcessPayment(GetPaymentProcessingRequest()));

            output.InnerException.Should().Be(genException);
        }
    }
}