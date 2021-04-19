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
        ///
        // If this code written for production, then we will have a seperate service which will validate against a database of existing users, but for the sake of this project
        // a dictonary is used to keep track of username and passwords which are allowed to use the two endpoints
        ///
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