using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.UserDTOs;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<BaseResponse<PagedResponse<UserResponse>>> GetUserList(UserRequest request)
    {
        IQueryable<User> query = _repository.GetUserQuery();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();

            query = query.Where(u =>
                u.Name.ToLower().Contains(search) ||
                u.Email.ToLower().Contains(search));
        }

        if (request.Role > 0)
        {
            query = query.Where(u => u.RoleId == request.Role);
        }

        query = query.ApplySorting(request.SortBy, request.SortDirection);

        var totalCount = await query.CountAsync();

        if (request.PageSize > 0)
        {
            query = query.Skip(request.Page * request.PageSize).Take(request.PageSize);
        }

        var users = await query.ToListAsync();

        List<UserResponse> records = users.Select(MapToResponse).ToList();

        PagedResponse<UserResponse> response = new()
        {
            Items = records,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
        };

        return ResponseFactory.Success(response, Messages.General.Success, StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<UserResponse>> GetUserById(Guid userId)
    {
        User? user = await _repository.GetUserById(userId);

        if (user == null)
        {
            return ResponseFactory.Failure<UserResponse>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        return ResponseFactory.Success(MapToResponse(user), Messages.General.Success, StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<object>> UpsertUser(UpsertUserRequest request)
    {
        if (request.UserId.HasValue)
        {
            User? existing = await _repository.GetUserById(request.UserId.Value);

            if (existing == null)
            {
                return ResponseFactory.Failure<object>(
                    Messages.General.NotFound,
                    StatusCodes.Status404NotFound);
            }

            // Check email conflict against another user
            User? emailConflict = await _repository.GetByEmailAsync(request.Email);
            if (emailConflict != null && emailConflict.UserId != existing.UserId)
            {
                return ResponseFactory.Failure<object>(
                    Messages.Auth.EmailAlreadyExists,
                    StatusCodes.Status409Conflict);
            }

            existing.Name = request.Name;
            existing.Email = request.Email;
            existing.RoleId = request.RoleId;
            existing.UpdatedOn = DateTime.UtcNow;
            existing.IsActive = request.IsActive;

            // Update password only if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                existing.PasswordHash = _passwordHasher.HashPassword(existing, request.Password);
            }

            await _repository.UpdateUser(existing);

            return ResponseFactory.Success<object>(new object(), Messages.User.UpdateSuccess, StatusCodes.Status200OK);
        }

        // CREATE
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return ResponseFactory.Failure<object>(
                Messages.User.PasswordRequired,
                StatusCodes.Status400BadRequest);
        }

        User? existingUser = await _repository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return ResponseFactory.Failure<object>(
                Messages.Auth.EmailAlreadyExists,
                StatusCodes.Status409Conflict);
        }

        var newUser = new User
        {
            UserId = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            RoleId = request.RoleId,
            IsActive = true,
            IsDeleted = false,
            CreatedOn = DateTime.UtcNow
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        await _repository.CreateUser(newUser);

        return ResponseFactory.Success<object>(new object(), Messages.User.CreateSuccess, StatusCodes.Status201Created);
    }

    public async Task<BaseResponse<object>> ToggleUserStatus(Guid userId)
    {
        User? user = await _repository.GetUserById(userId);

        if (user == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        user.IsActive = !user.IsActive;
        user.UpdatedOn = DateTime.UtcNow;

        await _repository.UpdateUser(user);

        return ResponseFactory.Success<object>(new object(), Messages.User.StatusToggled, StatusCodes.Status200OK);
    }

    public async Task<BaseResponse<object>> DeleteUser(Guid userId)
    {
        User? user = await _repository.GetUserById(userId);

        if (user == null)
        {
            return ResponseFactory.Failure<object>(
                Messages.General.NotFound,
                StatusCodes.Status404NotFound);
        }

        user.IsDeleted = true;
        user.IsActive = false;
        user.UpdatedOn = DateTime.UtcNow;

        await _repository.UpdateUser(user);

        return ResponseFactory.Success<object>(new object(), Messages.User.DeleteSuccess, StatusCodes.Status200OK);
    }

    private static UserResponse MapToResponse(User user) => new()
    {
        UserId = user.UserId,
        Name = user.Name,
        Email = user.Email,
        RoleId = user.RoleId,
        Role = user.Role?.RoleName ?? string.Empty,
        IsActive = user.IsActive,
        IsDeleted = user.IsDeleted
    };
}
