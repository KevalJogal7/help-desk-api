using HelpDesk.Domain.Entities;

namespace HelpDesk.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
