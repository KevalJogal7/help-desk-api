using HelpDesk.Domain.Entities;

namespace HelpDesk.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserById(Guid userId);
    Task<User?> CreateUser(User user);
    Task UpdateUser(User user);
    IQueryable<User> GetUserQuery();
}
