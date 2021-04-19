using System;
using NUnit.Framework;
using FluentAssertions;
using Moq;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.Banking.Models;
using PaymentGateway.Services.PaymentProcessor.Models;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Banking.Exceptions;
using PaymentGateway.Services.Storage.Exceptions;
using PaymentGateway.Services.PaymentProcessor.Exceptions;

namespace PaymentGateway.Services.Tests.PaymentProcessor
{
    public partial class PaymentProcessorServiceTests
    {
        [Test]
        public void GivenRequestIsNull_WhenICallProcessPayment_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.ThrowsAsync<ArgumentNullException>(()=> _sut.ProcessPayment(null));
            output.ParamName.Should().Be("request");
        }

        [Test]
        public void GivenBankingServiceReturnsSuccess_WhenICallProcessPayment_ThenSuccessfulPaymentProcessingResponseIsReturned()
        {
            BankingResponse bankingResponse = new BankingResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Successful = true
            };
            _mockedBankingService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ReturnsAsync(bankingResponse);

            var oneOfResponseOutput = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result;

            Assert.Multiple(()=>
            {
                oneOfResponseOutput.AsT0.Should().BeOfType<SuccessfulPaymentProcessing>();
                oneOfResponseOutput.AsT0.PaymentId.Should().Be(bankingResponse.Id);
                oneOfResponseOutput.IsT0.Should().BeTrue();
                oneOfResponseOutput.IsT1.Should().BeFalse();
                _mockedStorageService.Verify(x=>x.Upsert(
                    It.Is<string>(value => value == bankingResponse.Id), 
                    It.IsAny<PaymentAudit>()), Times.Once);
            });
        }

        [Test]
        public void GivenBankingServiceReturnsUnsuccesfull_WhenICallProcessPayment_ThenUnsuccessfulPaymentProcessingResponseIsReturned()
        {
            BankingResponse bankingResponse = new BankingResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Successful = false
            };
            _mockedBankingService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ReturnsAsync(bankingResponse);

            var oneOfResponseOutput = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result;

            Assert.Multiple(()=>
            {
                oneOfResponseOutput.AsT1.Should().BeOfType<UnsuccessfulPaymentProcessing>();
                oneOfResponseOutput.AsT1.Message.Should().Be($"Bank has denied the payment, please contact the bank and refer to this id : {bankingResponse.Id}");
                oneOfResponseOutput.IsT0.Should().BeFalse();
                oneOfResponseOutput.IsT1.Should().BeTrue();
                _mockedStorageService.Verify(x=>x.Upsert(
                    It.Is<string>(value => value == bankingResponse.Id), 
                    It.IsAny<PaymentAudit>()), Times.Once);
            });
        }

        [Test]
        public void GivenBankingServiceThrowsAnException_WhenICallProcessPayment_ThenPaymentProcessorServiceException()
        {
            var genException = new BankingServiceException("fake-error", new Exception("fake-error"));
            _mockedBankingService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ThrowsAsync(genException);
            
            var output = Assert.ThrowsAsync<PaymentProcessorServiceException>(()=> _sut.ProcessPayment(new PaymentProcessingRequest()));
             
            output.InnerException.Should().Be(genException);
        }

        [Test]
        public void GivenStorageServiceThrowsAnException_WhenICallProcessPayment_ThenPaymentProcessorServiceException()
        {
            var genException = new StorageException("fake-error", new Exception("fake-error"));
            BankingResponse bankingResponse = new BankingResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Successful = false
            };
            _mockedBankingService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ReturnsAsync(bankingResponse);
            _mockedStorageService.Setup(x => x.Upsert(It.IsAny<string>(), It.IsAny<PaymentAudit>())).ThrowsAsync(genException);

            var output = Assert.ThrowsAsync<PaymentProcessorServiceException>(()=> _sut.ProcessPayment(new PaymentProcessingRequest()));
             
            output.InnerException.Should().Be(genException);
        }
    }
}