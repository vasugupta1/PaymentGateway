using System.Collections.Generic;

namespace PaymentGateway.Services.AuthUsers.Interface
{
    public interface IUserService
    {
        bool IsValid(string username, string password);
    }
}