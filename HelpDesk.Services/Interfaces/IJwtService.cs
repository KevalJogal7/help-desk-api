using HelpDesk.Domain.Entities;

namespace HelpDesk.Services.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(User user);

    string GenerateToken();
}
