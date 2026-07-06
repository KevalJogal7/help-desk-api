using HelpDesk.Domain.Entities;

namespace HelpDesk.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetRoleByIdAsync(int roleId);

}
