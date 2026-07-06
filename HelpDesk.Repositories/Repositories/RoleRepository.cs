namespace HelpDesk.Repositories.Repositories;

using HelpDesk.Domain.Context;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetRoleByIdAsync(int roleId)
    {
        return await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId);
    }

}