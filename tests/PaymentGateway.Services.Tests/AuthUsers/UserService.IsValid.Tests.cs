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
        [TestCase("user","password")]
        [TestCase("user1","password1")]
        [TestCase("user2", "password123")]
        public void GivenIHaveValidUserNamePassword_WhenICallIsValid_ThenTrueResultIsReturned(string username, string password)
        {
            var output = _sut.IsValid(username, password);

            output.Should().BeTrue();
        }

        [TestCase("user","pas12sword")]
        [TestCase("user1","pas12sword1")]
        [TestCase("user2", "passwordsdsdd123")]
        public void GivenIHaveInvalidUserNamePassword_WhenICallIsValid_ThenFalseResultIsReturned(string username, string password)
        {
            var output = _sut.IsValid(username, password);

            output.Should().BeFalse();
        }

        [TestCase("user","password")]
        [TestCase("user1","password1")]
        [TestCase("user2", "password123")]
        public void GivenMyUsernameIsCorrectButMyPasswordsAreInTheWrongCase_WhenICallIsValid_ThenFalseResultIsReturned
        (string username, string password)
        {
            var output = _sut.IsValid(username, password.ToUpper());

            output.Should().BeFalse();
        }

        [TestCase("user","password")]
        [TestCase("user1","password1")]
        [TestCase("user2", "password123")]
        public void GivenIHaveUserNameAreInWrongCase_WhenICallIsValid_ThenTrueResultIsReturned(string username, string password)
        {
            var output = _sut.IsValid(username.ToUpper(), password);

            output.Should().BeFalse();
        }
    }
}