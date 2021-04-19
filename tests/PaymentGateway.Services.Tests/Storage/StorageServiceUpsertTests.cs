using Moq;
using FluentAssertions;
using NUnit.Framework;
using StackExchange.Redis;
using PaymentGateway.Services.Tests.Storage.Models;
using System;
using PaymentGateway.Services.Storage.Exceptions;

namespace PaymentGateway.Services.Tests.Storage
{
    public partial class StorageServiceTests
    {
        
        [TestCaseSource("EmptyKeyTestSource")]
        public void GivenKeyIsEmptyOrNull_WhenICallUpsert_ThenArgumentNullExceptionIsThrown(string key)
        {
            var output = Assert.ThrowsAsync<ArgumentNullException>(()=> _sut.Upsert(key, new DataModel()));
            output.ParamName.Should().Be("key");
        }
        [Test]
        public void GivenObjectIsEmptyOrNull_WhenICallUpsert_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.ThrowsAsync<ArgumentNullException>(()=> _sut.Upsert("fake-key", null));
            output.ParamName.Should().Be("dataObject");
        }

        [Test]
        public void GivenAValidKeyAndDataObject_WhenICallUpsert_ThenNoExceptionIsThrown()
        {
            //Arrange
            var expectedOutput = GetModel(GetStringData());
            var key = "fake-key";
            
            //Act & Assert
           Assert.DoesNotThrowAsync(()=> _sut.Upsert(key, expectedOutput)); 
           _mockedDatabase.Verify(x=>x.StringSetAsync(It.Is<RedisKey>(value => value.Equals(key)), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Test]
        public void GivenADatabaseThrowsException_WhenICallUpsert_ThenStorageExceptionIsThrown()
        {
            //Arrange
            var genException = new Exception("fake-exception");
            var input = GetModel(GetStringData());
            var key = "fake-key";
            _mockedDatabase.Setup(x => x.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, It.IsAny<When>(), It.IsAny<CommandFlags>())).ThrowsAsync(genException);
            
            //Act
            var output = Assert.ThrowsAsync<StorageException>(()=>_sut.Upsert(key, input));

            //Assert
            Assert.Multiple(()=>{
               output.InnerException.Should().Be(genException);
                
            });
        }
    }
}