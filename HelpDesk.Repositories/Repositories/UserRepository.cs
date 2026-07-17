namespace HelpDesk.Repositories.Repositories;

using HelpDesk.Domain.Context;
using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<User?> CreateUser(User user)
    {
        var entity = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return entity.Entity;
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public IQueryable<User> GetUserQuery()
    {
        return _context.Users.Include(u => u.Role).AsQueryable();
    }

}