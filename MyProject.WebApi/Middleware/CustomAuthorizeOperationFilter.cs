using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using MyProject.WebApi.Attributes;

namespace MyProject.WebApi.Middleware;

/// <summary>
/// Operation filter for Swagger to require JWT authentication on endpoints with [CustomAuthorize].
/// </summary>
public class CustomAuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasCustomAuthorize = context.MethodInfo.GetCustomAttribute<CustomAuthorizeAttribute>() != null ||
                                 context.MethodInfo.DeclaringType?.GetCustomAttribute<CustomAuthorizeAttribute>() != null;

        if (!hasCustomAuthorize) return;
        
        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}

