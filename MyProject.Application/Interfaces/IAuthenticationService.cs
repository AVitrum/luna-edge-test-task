using MyProject.Application.Payloads;

namespace MyProject.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<string?>> RegisterAsync(string username, string email, string password);
    Task<Result<string?>> AuthenticateAsync(string identifier, string password);
}