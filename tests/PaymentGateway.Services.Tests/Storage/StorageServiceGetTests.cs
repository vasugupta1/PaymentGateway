using Moq;
using FluentAssertions;
using NUnit.Framework;
using StackExchange.Redis;
using PaymentGateway.Services.Storage;
using PaymentGateway.Services.Tests.Storage.Models;
using System;
using System.Text.Json;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.Storage.Exceptions;

namespace PaymentGateway.Services.Tests.Storage
{
    public partial class StorageServiceTests
    {
        [TestCaseSource("EmptyKeyTestSource")]
        public void GivenKeyIsEmptyOrNull_WhenICallGet_ThenArgumentNullExceptionIsThrown(string key)
        {
            var output = Assert.ThrowsAsync<ArgumentNullException>(()=> _sut.Get(key));
            output.ParamName.Should().Be("key");
        }

        [Test]
        public void GivenAValidKey_WhenICallGet_ThenADataModelIsReturned()
        {
            //Arrange
            var inputData = GetStringData();
            var expectedOutput = GetModel(inputData);
            var key = "fake-key";
            _mockedDatabase.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(inputData);
            
            //Act
            var oneOfResponse = _sut.Get(key).Result;

            //Assert
            Assert.Multiple(()=>{
                oneOfResponse.AsT0.As<DataModel>().Should().BeEquivalentTo(expectedOutput);
                oneOfResponse.IsT1.Should().BeFalse();
            });
        }

        [Test]
        public void GivenANoDataIsPresentInDatabaseForGivenKey_WhenICallGet_ThenANotFoundResponseIsReturned()
        {
            //Arrange
            var key = "fake-key";
            _mockedDatabase.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(string.Empty);
            
            //Act
            var oneOfResponse = _sut.Get(key).Result;

            //Assert
            Assert.Multiple(()=>{
                oneOfResponse.IsT0.Should().BeFalse();
                oneOfResponse.IsT1.Should().BeTrue();
                oneOfResponse.AsT1.Should().BeOfType<NotFoundResponse>();
            });
        }

        [Test]
        public void GivenADatabaseThrowsException_WhenICallGet_ThenStorageExceptionIsThrown()
        {
            //Arrange
            var genException = new Exception("fake-exception");
            var key = "fake-key";
            _mockedDatabase.Setup(x => x.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ThrowsAsync(genException);
            
            //Act
            var output = Assert.ThrowsAsync<StorageException>(()=>_sut.Get(key));

            //Assert
            Assert.Multiple(()=>{
               output.InnerException.Should().Be(genException);
                
            });
        }
    }
}