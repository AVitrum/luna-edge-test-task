using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads.Requests;
using MyProject.Domain.Entities;
using MyProject.Domain.Enums;
using MyProject.WebApi.Attributes;

namespace MyProject.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly IAuthenticationService _userService;

    public UsersController(IAuthenticationService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request.Username, request.Email, request.Password);
        return StatusCode(result.Code, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
    {
        var result = await _userService.AuthenticateAsync(request.Identifier, request.Password);
        return StatusCode(result.Code, result);
    }
}