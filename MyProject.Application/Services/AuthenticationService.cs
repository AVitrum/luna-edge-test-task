using Microsoft.Extensions.Logging;
using MyProject.Application.Interfaces;
using MyProject.Application.Payloads;
using MyProject.Domain.Entities;
using MyProject.Domain.Interfaces;

namespace MyProject.Application.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<string?>> RegisterAsync(string username, string email, string password)
    {
        _logger.LogInformation("Attempting to register a new user with username {Username}", username);
        if (await _userRepository.ExistsByUsernameAsync(username))
        {
            _logger.LogWarning("Registration failed for username {Username}: username already exists.", username);
            return Result<string?>.Fail("Username already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email,
            PasswordHash = _passwordHasher.Hash(password),
        };

        await _userRepository.AddUserAsync(user);
        _logger.LogInformation("User {Username} added to the database.", username);

        user = await _userRepository.GetUserByUsernameAsync(user.Username);
        if (user == null)
        {
            _logger.LogError("Failed to retrieve user {Username} after registration.", username);
            return Result<string?>.Fail("User registration failed.");
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        _logger.LogInformation("User {Username} registered successfully.", user.Username);
        return new Result<string?>(true, "User registered successfully.", 200, token);
    }

    public async Task<Result<string?>> AuthenticateAsync(string identifier, string password)
    {
        _logger.LogInformation("Authentication attempt for identifier {Identifier}", identifier);
        User? user;

        if (identifier.Contains('@'))
        {
            _logger.LogInformation("Attempting to find user by email {Email}", identifier);
            user = await _userRepository.GetUserByEmailAsync(identifier);
        }
        else
        {
            _logger.LogInformation("Attempting to find user by username {Username}", identifier);
            user = await _userRepository.GetUserByUsernameAsync(identifier);
        }


        if (user is null || !_passwordHasher.Verify(password, user.PasswordHash))
        {
            _logger.LogWarning("Authentication failed for identifier {Identifier}. Invalid credentials.", identifier);
            return Result<string?>.Fail("Invalid username or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        _logger.LogInformation("User {Username} authenticated successfully.", user.Username);
        return new Result<string?>(true, "Authenticating...", 200, token);
    }
}