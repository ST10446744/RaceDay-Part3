using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RaceDay.MVC.Filters;

public sealed class SessionAuthorizeAttribute : Attribute, IAsyncActionFilter
{
    private readonly string? _requiredRole;

    public SessionAuthorizeAttribute(string? requiredRole = null)
    {
        _requiredRole = requiredRole;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var session = context.HttpContext.Session;
        var userId = session.GetString("UserId");

        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Result = new RedirectToActionResult(
                "Login",
                "Account",
                null);
            return;
        }

        if (_requiredRole != null &&
            !string.Equals(
                session.GetString("Role"),
                _requiredRole,
                StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new StatusCodeResult(
                StatusCodes.Status403Forbidden);
            return;
        }

        await next();
    }
}
