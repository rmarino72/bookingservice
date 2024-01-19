using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace bookingservice.security;

public sealed class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    // Override this method to implement your token validation 
    private bool ValidateToken(string token)
    {
        return token == "booking_service_token";
    }

    /**
     * Bearer Token authorization checker
     */
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Parameter is null)
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            var credentials = authHeader.Parameter.Split(new[] { ' ' }, 2);
            var token = credentials[0];

            if (!ValidateToken(token)) return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
            var claims = new Claim[]
            {
                new(ClaimTypes.NameIdentifier, token)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}