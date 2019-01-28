using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using CoreSecurityPrototype.Data;
using CoreSecurityPrototype.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreSecurityPrototype.Authentication
{
    public class AuthorizationProvider : OpenIdConnectServerProvider
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationProvider(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    description: "Only the resource owner password credentials and refresh token " +
                                 "grants are accepted by this authorization server.");

                return Task.FromResult(0);
            }

            context.Skip();

            return Task.FromResult(0);
        }

        public override async Task HandleTokenRequest(HandleTokenRequestContext context)
        {
            if (!context.Request.IsPasswordGrantType())
                return;

            var username = context.Request.Username;
            var password = context.Request.Password;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || await _userManager.CheckPasswordAsync(user, password))
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidGrant,
                    description: "Invalid user credentials."
                );

                return;
            }

            var identity = new ClaimsIdentity(
                OpenIdConnectServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role
            );

            identity.AddClaim(
                OpenIdConnectConstants.Claims.Subject,
                user.Id.ToString(),
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken
            );

            identity.AddClaim(
                OpenIdConnectConstants.Claims.Name,
                user.UserName.ToString(),
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken
            );

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme
            );

            ticket.SetScopes(
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess
            );

            context.Validate(ticket);

            return;
        }
    }
}
