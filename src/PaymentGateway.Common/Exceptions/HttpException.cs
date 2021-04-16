using System;
using System.Net;

namespace PaymentGateway.Common.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode;
        public HttpException(string message, Exception innerException, HttpStatusCode statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}