using MyProject.Application.Interfaces;
using MyProject.Application.Payloads;
using MyProject.Domain.Entities;
using MyProject.Domain.Enums;
using MyProject.Domain.Interfaces;

namespace MyProject.Application.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<string?>> RegisterAsync(string username, string email, string password)
    {
        if (await _userRepository.ExistsByUsernameAsync(username))
        {
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
        
        user = await _userRepository.GetUserByUsernameAsync(user.Username);
        return user == null
            ? Result<string?>.Fail("User registration failed.")
            : new Result<string?>(true, "User registered successfully.", 200, _jwtTokenGenerator.GenerateToken(user));
    }

    public async Task<Result<string?>> AuthenticateAsync(string identifier, string password)
    {
        User? user;
        
        if (identifier.Contains('@'))
        {
            user = await _userRepository.GetUserByEmailAsync(identifier);
        }
        else
        {
            user = await _userRepository.GetUserByUsernameAsync(identifier);
        }
        

        if (user is null || !_passwordHasher.Verify(password, user.PasswordHash))
        {
            return Result<string?>.Fail("Invalid username or password.");
        }

        return new Result<string?>(true, "Authenticating...", 200, _jwtTokenGenerator.GenerateToken(user));
    }
}