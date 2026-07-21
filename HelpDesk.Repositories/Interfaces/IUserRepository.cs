using HelpDesk.Domain.Entities;

namespace HelpDesk.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetUserById(Guid userId);
    Task<User?> CreateUser(User user);
    Task UpdateUser(User user);
    IQueryable<User> GetUserQuery();
    Task AddRefreshToken(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task AddResetPasswordToken(ResetPasswordToken token);
    Task UpdateResetPasswordToken(ResetPasswordToken token);
    Task<ResetPasswordToken?> GetByResetPasswordTokenAsync(string token);
}
