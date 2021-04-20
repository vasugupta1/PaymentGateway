using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using OneOf;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.PaymentProcessor.Exceptions;
using PaymentGateway.Services.PaymentProcessor.Models;

namespace PaymentGateway.API.Tests.Controllers
{
    public partial class PaymentProcessorControllerTests
    {
        [Test]
        public void GivenValidRequest_WhenICallProcessPayment_ThenOkObjectResultIsReturned()
        {
            SuccessfulPaymentProcessing expectedResponse = new SuccessfulPaymentProcessing()
            {
                PaymentId = Guid.NewGuid().ToString(),
                Success = true, 
                Message = "worked"
            };
            OneOf<SuccessfulPaymentProcessing, UnsuccessfulPaymentProcessing> oneOfResponse = expectedResponse;
            _mockedPaymentProcessorService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ReturnsAsync(oneOfResponse);

            var output = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result as OkObjectResult;

            output.Value.Should().Be(expectedResponse);
        }

        [Test]
        public void GivenProcessingWasUnsuccssfull_WhenICallProcessPayment_ThenBadRequestObjectResultIsReturned()
        {
            UnsuccessfulPaymentProcessing expectedResponse = new UnsuccessfulPaymentProcessing()
            {
                Success = false, 
                Message = "didn't work"
            };
            OneOf<SuccessfulPaymentProcessing, UnsuccessfulPaymentProcessing> oneOfResponse = expectedResponse;
            _mockedPaymentProcessorService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ReturnsAsync(oneOfResponse);

            var output = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result as BadRequestObjectResult;

            output.Value.Should().Be(expectedResponse);
        }

        [Test]
        public void GivenPaymentProcesserThrowsAnPaymentProcessorServiceException_WhenICallProcessPayment_Then500StatusCodeReturned()
        {
            var exception = new PaymentProcessorServiceException("failed", new Exception("failed"));
            _mockedPaymentProcessorService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ThrowsAsync(exception);

            var output = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result as ObjectResult;

            Assert.Multiple(()=>
            {
                output.StatusCode.Should().Be(500);
                _mockedLogger.Verify(m => m.Log(
                            LogLevel.Error,
                            It.IsAny<EventId>(),
                            It.Is<It.IsAnyType>((v, t) => true),
                            It.IsAny<Exception>(),
                            (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                        Times.Once);
            });
        }

        [Test]
        public void GivenPaymentProcesserThrowsAnException_WhenICallProcessPayment_Then500StatusCodeReturned()
        {
            var exception = new Exception("failed");
            _mockedPaymentProcessorService.Setup(x => x.ProcessPayment(It.IsAny<PaymentProcessingRequest>())).ThrowsAsync(exception);

            var output = _sut.ProcessPayment(GetPaymentProcessingRequest()).Result as ObjectResult;

            Assert.Multiple(()=>
            {
                output.StatusCode.Should().Be(500);
                _mockedLogger.Verify(m => m.Log(
                            LogLevel.Error,
                            It.IsAny<EventId>(),
                            It.Is<It.IsAnyType>((v, t) => true),
                            It.IsAny<Exception>(),
                            (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                        Times.Once);
            });
        }
    }
}
