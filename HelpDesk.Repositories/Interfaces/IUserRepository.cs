using HelpDesk.Domain.Entities;

namespace HelpDesk.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> CreateUser(User user);
}
