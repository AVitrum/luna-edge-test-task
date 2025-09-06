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
    /// Route: POST /users/register<br/>
    /// Authentication: Not required<br/>
    /// <b>Acceptable variables:</b><br/>
    /// <ul>
    ///   <li><b>Username</b> (string, required): Only letters, numbers, and <c>_</c>, <c>.</c>, <c>-</c>.</li>
    ///   <li><b>Email</b> (string, required): Must be a valid email address.</li>
    ///   <li><b>Password</b> (string, required): At least 8 characters, must contain at least one uppercase letter, one lowercase letter, and one number.</li>
    /// </ul>
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
    /// Route: POST /users/login<br/>
    /// Authentication: Not required<br/>
    /// <b>Acceptable variables:</b><br/>
    /// <ul>
    ///   <li><b>Identifier</b> (string, required): Username or email.</li>
    ///   <li><b>Password</b> (string, required): User's password.</li>
    /// </ul>
    /// </remarks>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateRequest request)
    {
        var result = await _userService.AuthenticateAsync(request.Identifier, request.Password);
        return StatusCode(result.Code, result);
    }
}