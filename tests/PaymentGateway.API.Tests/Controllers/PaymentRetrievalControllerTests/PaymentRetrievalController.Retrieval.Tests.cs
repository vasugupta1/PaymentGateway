using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Common.Models.Storage;
using System;
using NUnit.Framework;
using FluentAssertions;
using OneOf;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Services.Storage.Exceptions;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway.API.Tests.Controllers.PaymentRetrievalControllerTests
{
    public partial class PaymentRetrievalControllerTests
    {
        [TestCaseSource("EmptyOrNullStrings")]
        public void GivenEmptyOrNullString_WhenICallRetrievePayment_ThenArgumentNullExceptionIsThrown(string id)
        {
            var output = Assert.ThrowsAsync<ArgumentNullException>( () => _sut.RetrievePayment(id));

            output.ParamName.Should().Be("id");
        }

        [Test]
        public void GivenAValidId_WhenICallRetrievePayment_ThenAPaymentAuditIsReturned()
        {
            //Arrange
            var paymentAudit = GetPaymentAudit();
            OneOf<PaymentAudit, NotFoundResponse> expectedOneOfResponse = paymentAudit;
            _mockedStorageService.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(expectedOneOfResponse);

            //Act
            var output = _sut.RetrievePayment("fake-id").Result;
            var result = output as OkObjectResult;

            //Assert
            result.Value.Should().Be(paymentAudit);
        }

        [Test]
        public void GivenAIdDoesntExistInDatabase_WhenICallRetrievePayment_ThenANotFoundResultIsReturned()
        {
            //Arrange
            var notfoundResponse = GetNotFoundResponse();
            OneOf<PaymentAudit, NotFoundResponse> expectedOneOfResponse = notfoundResponse;
            _mockedStorageService.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(expectedOneOfResponse);

            //Act
            var output = _sut.RetrievePayment("fake-id").Result;
            var result = output as NotFoundObjectResult;

            //Assert
            result.Value.Should().Be(notfoundResponse);
        }

        [Test]
        public void GivenStorageServicesThrowsException_WhenICallRetrievePayment_ThenAErrorIsLoggedAnd500StatusIsReturned()
        {
            //Arrange
            var exception = new StorageException("fake", new Exception("fake"));

            _mockedStorageService.Setup(x => x.Get(It.IsAny<string>())).ThrowsAsync(exception);

            //Act
            var output = _sut.RetrievePayment("fake-id").Result  as ObjectResult ; 

            Assert.Multiple(() =>
            {
                output.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
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
        public void GivenExceptionThrows_WhenICallRetrievePayment_ThenAErrorIsLoggedAnd500StatusIsReturned()
        {
            //Arrange
            var exception = new Exception("fake");

            _mockedStorageService.Setup(x => x.Get(It.IsAny<string>())).ThrowsAsync(exception);

            //Act
            var output = _sut.RetrievePayment("fake-id").Result  as ObjectResult ; 

            Assert.Multiple(() =>
            {
                output.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
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
