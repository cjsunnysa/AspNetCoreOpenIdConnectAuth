using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreSecurityPrototype.Authentication
{
    public static class PrototypeTokenServer
    {
        public static void Configure(OpenIdConnectServerOptions options)
        {
            options.TokenEndpointPath = "/connect/token";
            options.AccessTokenLifetime = TimeSpan.FromHours(1);
            options.RefreshTokenLifetime = null;

            options.Provider.OnHandleTokenRequest = HandleTokenRequest;
            options.Provider.OnValidateTokenRequest = ValidateTokenRequest;
        }


        private static Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only grant_type=password and refresh_token " +
                                 "requests are accepted by this server.");

                return Task.CompletedTask;
            }

            if (string.Equals(context.ClientId, "prototype_client") &&
                string.Equals(context.Request.Username, "Bob", StringComparison.Ordinal) &&
                string.Equals(context.Request.Password, "Password123!"))
            {
                context.Validate();
            }

            return Task.CompletedTask;
        }

        private static Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType())
                return Task.CompletedTask;

            if (!string.Equals(context.Request.Username, "Bob", StringComparison.Ordinal) ||
                !string.Equals(context.Request.Password, "Password123!", StringComparison.Ordinal))
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Invalid user credentials."
                );

                return Task.CompletedTask;
            }

            var identity = new ClaimsIdentity(
                context.Scheme.Name,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role
            );

            var subjectClaim = new Claim(OpenIdConnectConstants.Claims.Subject, "[user_id]");

            identity.AddClaim(subjectClaim);

            var serializedDestinationClaim = new Claim(
                "urn:customclaim",
                "value",
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken
            );

            identity.AddClaim(serializedDestinationClaim);

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                context.Scheme.Name
            );

            ticket.SetScopes(
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess
            );

            context.Validate(ticket);

            return Task.CompletedTask;
        }
    }
}
