using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.API.Controllers.v1;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using AutoFixture;

namespace PaymentGateway.API.Tests.Controllers.PaymentRetrievalControllerTests
{
    public partial class PaymentRetrievalControllerTests
    {
        private protected Mock<IStorageService<PaymentAudit>> _mockedStorageService;
        private protected Mock<ILogger<PaymentRetrievalController>> _mockedLogger;
        private protected PaymentRetrievalController _sut;
        
        [SetUp]
        public void SetUp()
        {
            _mockedLogger = new Mock<ILogger<PaymentRetrievalController>>();
            _mockedStorageService = new Mock<IStorageService<PaymentAudit>>();
            _sut = new PaymentRetrievalController(_mockedLogger.Object, _mockedStorageService.Object);
        }

        [Test]
        public void GivenLoggerIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new PaymentRetrievalController(null, _mockedStorageService.Object));

            output.ParamName.Should().Be("logger");
        }

        [Test]
        public void GivenStorageServiceIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new PaymentRetrievalController(_mockedLogger.Object, null));

            output.ParamName.Should().Be("storageService");
        }

        private static string[] EmptyOrNullStrings => new string[]{ "", null };

        private PaymentAudit GetPaymentAudit()
        {
            return new Fixture().Create<PaymentAudit>();
        }

        private NotFoundResponse GetNotFoundResponse()
        {
            return new NotFoundResponse()
            {
                ErrorMessage = "Not Found"
            };
        }

    }
}
