using System;
namespace PaymentGateway.Services.Banking.Exceptions
{
    public class BankingServiceException : Exception
    {
        public BankingServiceException(string message, Exception innerException): base(message, innerException)
        {
            
        }
    }
}