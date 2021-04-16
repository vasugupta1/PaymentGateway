using System;
using System.Collections.Generic;
using PaymentGateway.Services.AuthUsers.Interface;

namespace PaymentGateway.Services.AuthUsers
{
    public class UserService : IUserService
    {
        private readonly IReadOnlyDictionary<string, string>  _usernamesAndPassword;
        public UserService(IReadOnlyDictionary<string, string> usernamesAndPassword)
        {
            _usernamesAndPassword = usernamesAndPassword ?? throw new ArgumentNullException(nameof(usernamesAndPassword));
        }
        public bool IsValid(string username, string password)
        {
            if(_usernamesAndPassword.ContainsKey(username) && _usernamesAndPassword[username].Equals(password, StringComparison.CurrentCulture))
            {
                return true;
            }
            return false;
        }
    }
}