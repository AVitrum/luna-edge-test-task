using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyProject.Domain.Interfaces;
using MyProject.Infrastructure.Settings;

namespace MyProject.WebApi.Middleware;

/// <summary>
/// Middleware for validating JWT tokens and attaching the authenticated user to the request context.
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSettings _jwtSettings;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="JwtMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="jwtSettings">The JWT settings.</param>
    public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Invokes the middleware to validate the JWT token and attach the user to the context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context, IUserRepository userRepository)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            await AttachUserToContext(context, userRepository, token);
        }
        await _next(context);
    }

    /// <summary>
    /// Validates the JWT token and attaches the user to the request context.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="token">The JWT token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AttachUserToContext(HttpContext context, IUserRepository userRepository, string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var username = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
            
        context.Items["User"] = await userRepository.GetUserByUsernameAsync(username);
    }
}