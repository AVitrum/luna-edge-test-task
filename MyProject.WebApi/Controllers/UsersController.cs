using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads.Requests;

namespace MyProject.WebApi.Controllers;

/// <summary>
/// Controller for user registration and authentication.
/// Provides endpoints for registering new users and authenticating existing users.
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IAuthenticationService _userService;

    public UsersController(IAuthenticationService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The registration request payload.</param>
    /// <returns>Result of the registration operation.</returns>
    /// <remarks>
    /// Route: POST /users/register
    /// Authentication: Not required
    /// </remarks>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request.Username, request.Email, request.Password);
        return StatusCode(result.Code, result);
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="request">The authentication request payload.</param>
    /// <returns>Result of the authentication operation.</returns>
    /// <remarks>
    /// Route: POST /users/login
    /// Authentication: Not required
    /// </remarks>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequest request)
    {
        var result = await _userService.AuthenticateAsync(request.Identifier, request.Password);
        return StatusCode(result.Code, result);
    }
}