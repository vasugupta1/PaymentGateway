using Moq;
using FluentAssertions;
using NUnit.Framework;
using StackExchange.Redis;
using PaymentGateway.Services.Storage;
using PaymentGateway.Services.Tests.Storage.Models;
using System;
using System.Text.Json;

namespace PaymentGateway.Services.Tests.Storage
{
    public partial class StorageServiceTests
    {
        private protected Mock<IDatabase> _mockedDatabase;
        private protected StorageService<DataModel> _sut;
        [SetUp]
        public void SetUp()
        {
            _mockedDatabase = new Mock<IDatabase>();
            _sut = new StorageService<DataModel>(_mockedDatabase.Object);
        }

        [Test]
        public void GivenIDatabaseIsNull_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new StorageService<DataModel>(null));
            output.ParamName.Should().Be("database");
        }
        private string GetStringData()
        {
            return JsonSerializer.Serialize(new DataModel());
        }

        private DataModel GetModel(string inputData)
        {
            return JsonSerializer.Deserialize<DataModel>(inputData);
        }
        private static string[] EmptyKeyTestSource => new string[]{"", null};
    }
}