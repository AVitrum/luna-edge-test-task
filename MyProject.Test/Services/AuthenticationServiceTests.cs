using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyProject.Application.Interfaces;
using MyProject.Application.Services;
using MyProject.Domain.Entities;
using MyProject.Domain.Interfaces;
using NUnit.Framework;
using Task = System.Threading.Tasks.Task;

namespace MyProject.Application.Test.Services;

[TestFixture]
public sealed class AuthenticationServiceTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IPasswordHasher> _mockPasswordHasher;
    private Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private Mock<ILogger<AuthenticationService>> _mockLogger;
    private AuthenticationService _authService;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _mockLogger = new Mock<ILogger<AuthenticationService>>();
        _authService = new AuthenticationService(
            _mockUserRepository.Object,
            _mockPasswordHasher.Object,
            _mockJwtTokenGenerator.Object,
            _mockLogger.Object);
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnSuccess_WhenUserIsNew()
    {
        var username = "newuser";
        var email = "newuser@example.com";
        var password = "Password123!";
        var passwordHash = "hashed";
        var user = new User { Id = Guid.NewGuid(), Username = username, Email = email, PasswordHash = passwordHash };
        var token = "jwt-token";

        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(username)).ReturnsAsync(false);
        _mockPasswordHasher.Setup(h => h.Hash(password)).Returns(passwordHash);
        _mockUserRepository.Setup(r => r.AddUserAsync(It.IsAny<User>())).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        _mockJwtTokenGenerator.Setup(g => g.GenerateToken(user)).Returns(token);

        var result = await _authService.RegisterAsync(username, email, password);

        result.Success.Should().BeTrue();
        result.Data.Should().Be(token);
        result.Message.Should().Be("User registered successfully.");
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnFail_WhenUsernameExists()
    {
        var username = "existinguser";
        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(username)).ReturnsAsync(true);

        var result = await _authService.RegisterAsync(username, "email", "password");

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Username already exists.");
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnFail_WhenUserCannotBeRetrieved()
    {
        var username = "newuser";
        var email = "newuser@example.com";
        var password = "Password123!";
        var passwordHash = "hashed";

        _mockUserRepository.Setup(r => r.ExistsByUsernameAsync(username)).ReturnsAsync(false);
        _mockPasswordHasher.Setup(h => h.Hash(password)).Returns(passwordHash);
        _mockUserRepository.Setup(r => r.AddUserAsync(It.IsAny<User>())).ReturnsAsync(true);
        _mockUserRepository.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync((User)null!);

        var result = await _authService.RegisterAsync(username, email, password);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("User registration failed.");
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnFail_WhenUsernameIsNullOrEmpty()
    {
        var email = "user@example.com";
        var password = "Password123!";

        var resultNull = await _authService.RegisterAsync(null!, email, password);
        var resultEmpty = await _authService.RegisterAsync("", email, password);
        var resultWhitespace = await _authService.RegisterAsync("   ", email, password);

        resultNull.Success.Should().BeFalse();
        resultNull.Message.Should().Be("Username is required.");
        resultEmpty.Success.Should().BeFalse();
        resultEmpty.Message.Should().Be("Username is required.");
        resultWhitespace.Success.Should().BeFalse();
        resultWhitespace.Message.Should().Be("Username is required.");
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnFail_WhenEmailIsNullOrEmpty()
    {
        var username = "user";
        var password = "Password123!";

        var resultNull = await _authService.RegisterAsync(username, null!, password);
        var resultEmpty = await _authService.RegisterAsync(username, "", password);
        var resultWhitespace = await _authService.RegisterAsync(username, "   ", password);

        resultNull.Success.Should().BeFalse();
        resultNull.Message.Should().Be("Email is required.");
        resultEmpty.Success.Should().BeFalse();
        resultEmpty.Message.Should().Be("Email is required.");
        resultWhitespace.Success.Should().BeFalse();
        resultWhitespace.Message.Should().Be("Email is required.");
    }

    [Test]
    public async Task AuthenticateAsync_ShouldReturnSuccess_WhenCredentialsAreValid_ByUsername()
    {
        var username = "user1";
        var password = "Password123!";
        var passwordHash = "hashed";
        var user = new User { Id = Guid.NewGuid(), Username = username, Email = "", PasswordHash = passwordHash };
        var token = "jwt-token";

        _mockUserRepository.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(password, passwordHash)).Returns(true);
        _mockJwtTokenGenerator.Setup(g => g.GenerateToken(user)).Returns(token);

        var result = await _authService.AuthenticateAsync(username, password);

        result.Success.Should().BeTrue();
        result.Data.Should().Be(token);
        result.Message.Should().Be("Authenticating...");
    }

    [Test]
    public async Task AuthenticateAsync_ShouldReturnSuccess_WhenCredentialsAreValid_ByEmail()
    {
        var email = "user1@example.com";
        var password = "Password123!";
        var passwordHash = "hashed";
        var user = new User { Id = Guid.NewGuid(), Username = "", Email = email, PasswordHash = passwordHash };
        var token = "jwt-token";

        _mockUserRepository.Setup(r => r.GetUserByEmailAsync(email)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(password, passwordHash)).Returns(true);
        _mockJwtTokenGenerator.Setup(g => g.GenerateToken(user)).Returns(token);

        var result = await _authService.AuthenticateAsync(email, password);

        result.Success.Should().BeTrue();
        result.Data.Should().Be(token);
        result.Message.Should().Be("Authenticating...");
    }

    [Test]
    public async Task AuthenticateAsync_ShouldReturnFail_WhenUserNotFound()
    {
        var identifier = "unknownuser";
        _mockUserRepository.Setup(r => r.GetUserByUsernameAsync(identifier)).ReturnsAsync((User)null!);

        var result = await _authService.AuthenticateAsync(identifier, "password");

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Invalid username or password.");
    }

    [Test]
    public async Task AuthenticateAsync_ShouldReturnFail_WhenPasswordIsInvalid()
    {
        var username = "user1";
        var password = "wrongpassword";
        var user = new User { Id = Guid.NewGuid(), Username = username, Email = "", PasswordHash = "hashed" };

        _mockUserRepository.Setup(r => r.GetUserByUsernameAsync(username)).ReturnsAsync(user);
        _mockPasswordHasher.Setup(h => h.Verify(password, user.PasswordHash)).Returns(false);

        var result = await _authService.AuthenticateAsync(username, password);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Invalid username or password.");
    }
}
