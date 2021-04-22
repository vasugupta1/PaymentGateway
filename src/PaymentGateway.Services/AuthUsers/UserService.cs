using System;
using System.Collections.Generic;
using PaymentGateway.Services.AuthUsers.Interface;

namespace PaymentGateway.Services.AuthUsers
{
    public class UserService : IUserService
    {
        private readonly IReadOnlyDictionary<string, string>  _usernamesAndPasswordDatabase;
        public UserService(IReadOnlyDictionary<string, string> usernamesAndPasswordDatabase)
        {
            _usernamesAndPasswordDatabase = usernamesAndPasswordDatabase 
            ?? throw new ArgumentNullException(nameof(usernamesAndPasswordDatabase));
        }
        ///
        // If this code written for production, then we will have a seperate service which       
        //will communicate with a external database that will contain a list of usersnames
        //and passwords, but for the sake of this project a dictonary is used to keep track of 
        //username and passwords which are allowed to use the two endpoints. This dictinary of username 
        //and passwords can be found in appsettings.json file under the CustomConfiguration:Authentication property
        ///
        public bool IsValid(string username, string password)
        {
            if(_usernamesAndPasswordDatabase.ContainsKey(username) && 
                _usernamesAndPasswordDatabase[username].Equals(password, StringComparison.CurrentCulture))
            {
                return true;
            }
            return false;
        }
    }
}
