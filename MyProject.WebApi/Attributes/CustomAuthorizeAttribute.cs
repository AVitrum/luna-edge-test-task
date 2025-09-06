using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyProject.Domain.Entities;

namespace MyProject.WebApi.Attributes;

/// <summary>
/// Custom authorization attribute for validating user authentication in ASP.NET Core controllers and actions.
/// Returns 401 Unauthorized if the user is not authenticated.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Called during authorization to validate if the user is authenticated.
    /// </summary>
    /// <param name="context">The authorization filter context.</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["User"] is not User)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}