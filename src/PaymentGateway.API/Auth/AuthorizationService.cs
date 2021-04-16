using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Services.AuthUsers.Interface;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace PaymentGateway.API.Auth
{
    public class AuthorizationService : IAuthorizationFilter
    {
        private protected readonly string _realm;
        public AuthorizationService(string realm)
        {
            _realm = !string.IsNullOrEmpty(realm) ? realm : throw new ArgumentNullException(nameof(realm));
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];

                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var authCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty)).Split(':');

                        if (authCredentials.Length == 2)
                        {
                            if (IsAuthorized(context, authCredentials[0], authCredentials[1]))
                            {
                                return;
                            }
                        }
                    }
                }

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }
        public bool IsAuthorized(AuthorizationFilterContext context, string username, string password)
        {
            var userService = GetUserService(context);
            return userService.IsValid(username, password);
        }

        private IUserService GetUserService(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetRequiredService<IUserService>();
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            // Return 401 and a basic authentication challenge (causes browser to show login dialog)
            context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
            context.Result = new UnauthorizedResult();
        }
    }
}