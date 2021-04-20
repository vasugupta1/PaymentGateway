using System;
using System.Collections.Generic;
using NUnit.Framework;
using PaymentGateway.Services.AuthUsers;
using PaymentGateway.Services.AuthUsers.Interface;
using FluentAssertions;

namespace PaymentGateway.Services.Tests.AuthUsers
{
    public partial class UserServiceTests
    {
        private protected IReadOnlyDictionary<string, string>  _usernamesAndPasswordDatabase;
        private protected IUserService _sut;

        [SetUp]
        public void SetUp()
        {
            _usernamesAndPasswordDatabase = new Dictionary<string, string>()
            {
                {"user","password"},
                {"user1", "password1"},
                {"user2", "password123"}                
            };
            _sut = new UserService(_usernamesAndPasswordDatabase);
        }

        [Test]
        public void GivenDictionaryIsNullOrEmpty_WhenICallConstructor_ThenArgumentNullExceptionIsThrown()
        {
            var output = Assert.Throws<ArgumentNullException>(()=> new UserService(null));

            output.ParamName.Should().Be("usernamesAndPasswordDatabase");
        }
    }
}