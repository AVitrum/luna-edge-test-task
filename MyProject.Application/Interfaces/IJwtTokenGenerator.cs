using MyProject.Domain.Entities;

namespace MyProject.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}