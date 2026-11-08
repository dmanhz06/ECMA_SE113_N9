using System.Security.Claims;
using ECMA.Domain.Enums;
using ECMA.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ECMA.Web.Services;

public sealed class AuthCookieService(IUnitOfWork unitOfWork, AuthSessionStore sessions)
{
    public async Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        var idValue = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        var stamp = context.Principal?.FindFirstValue("security_stamp");
        var sessionId = context.Principal?.FindFirstValue("session_id");
        if (!Guid.TryParse(idValue, out var id) || !sessions.TryTouch(sessionId))
        {
            await RejectAsync(context, sessionId);
            return;
        }

        var user = await unitOfWork.Users.FindByIdAsync(id, context.HttpContext.RequestAborted);
        if (user is null || user.Status != UserStatus.Active ||
            !string.Equals(user.SecurityStamp, stamp, StringComparison.Ordinal))
        {
            await RejectAsync(context, sessionId);
            return;
        }

        var principal = context.Principal!;
        var currentRole = principal.FindFirstValue(ClaimTypes.Role);
        if (!string.Equals(currentRole, user.Role.ToString(), StringComparison.Ordinal))
        {
            var identity = (ClaimsIdentity)principal.Identity!;
            foreach (var claim in identity.FindAll(ClaimTypes.Role).ToArray()) identity.RemoveClaim(claim);
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
            context.ReplacePrincipal(principal);
        }

        context.ShouldRenew = true;
    }

    public static ClaimsPrincipal CreatePrincipal(Guid id, string username, string fullName, string role, string stamp, string sessionId)
    {
        var identity = new ClaimsIdentity(
        [
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.GivenName, fullName),
            new(ClaimTypes.Role, role),
            new("security_stamp", stamp),
            new("session_id", sessionId)
        ], CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    private async Task RejectAsync(CookieValidatePrincipalContext context, string? sessionId)
    {
        sessions.End(sessionId);
        context.RejectPrincipal();
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}