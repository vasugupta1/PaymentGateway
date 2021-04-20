using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PaymentGateway.API.Controllers.v1;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.PaymentProcessor.Interface;

namespace PaymentGateway.API.Tests.Controllers
{
    public partial class PaymentProcessorControllerTests
    {
        private protected Mock<IPaymentProcessorService> _mockedPaymentProcessorService;
        private protected Mock<ILogger<PaymentProcessorController>> _mockedLogger;
        private protected PaymentProcessorController _sut;
        
        [SetUp]
        public void SetUp()
        {
            _mockedLogger = new Mock<ILogger<PaymentProcessorController>>();
            _mockedPaymentProcessorService = new Mock<IPaymentProcessorService>();
            _sut = new PaymentProcessorController(_mockedLogger.Object, _mockedPaymentProcessorService.Object);
        }

        [Test]
        public void GivenLoggerIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(() => 
                new PaymentProcessorController(null, _mockedPaymentProcessorService.Object));

            output.ParamName.Should().Be("logger");
        }

        [Test]
        public void GivenPaymentProcessorControllerIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> 
                new PaymentProcessorController(_mockedLogger.Object, null));

            output.ParamName.Should().Be("paymentProcessorService");
        }

        private static List<PaymentProcessingRequest> BadRequestSource = new List<PaymentProcessingRequest>()
        {
            {
                new PaymentProcessingRequest()
                {
                    CurrencyCode = "",
                    ExpiryMonth = 123123,
                    ExpiryYear = 12323123,
                    CVV = 123123,
                    CardNumber = "123123123123123123",
                    Amount = -0.01
                }
            }
        };
    }
}
