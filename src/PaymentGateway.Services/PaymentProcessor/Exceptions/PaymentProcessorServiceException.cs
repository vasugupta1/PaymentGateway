using System;

namespace PaymentGateway.Services.PaymentProcessor.Exceptions
{
    public class PaymentProcessorServiceException : Exception
    {
        public PaymentProcessorServiceException(string message, Exception innerException): base(message, innerException)
        {
            
        }
    
    }
}