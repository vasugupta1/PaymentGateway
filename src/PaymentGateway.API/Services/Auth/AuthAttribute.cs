using System;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.API.Services.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BasicAuthAttribute : TypeFilterAttribute
    {
        public BasicAuthAttribute(string realm = @"PaymentGateway-Authentication") : base(typeof(AuthorizationService))
        {
            Arguments = new object[] { realm };
        }
    }
}