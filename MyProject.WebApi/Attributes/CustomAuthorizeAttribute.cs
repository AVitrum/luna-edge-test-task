using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyProject.Domain.Entities;
using MyProject.Domain.Enums;

namespace MyProject.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<UserRole> _roles;

    public CustomAuthorizeAttribute(params UserRole[]? roles)
    {
        _roles = roles ?? [];
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items["User"] is not User user)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        // if (_roles.Any() && !_roles.Contains(user.Role))
        // {
        //     context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
        // }
    }
}