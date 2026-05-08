using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ProjectBoard.Web.Auth;

public class DemoAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DemoAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Demo-User", out var userName))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var role = Request.Headers.TryGetValue("X-Demo-Role", out var roleValue)
            ? roleValue.ToString()
            : "User";

        var permission = Request.Headers.TryGetValue("X-Demo-Permission", out var permissionValue)
            ? permissionValue.ToString()
            : string.Empty;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userName.ToString()),
            new(ClaimTypes.Role, role),
            new("department", "Education")
        };

        if (!string.IsNullOrWhiteSpace(permission))
        {
            claims.Add(new Claim("permission", permission));
        }

        var identity = new ClaimsIdentity(claims, DemoAuthenticationDefaults.Scheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, DemoAuthenticationDefaults.Scheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
