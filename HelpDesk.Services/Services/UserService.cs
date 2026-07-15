using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.UserDTOs;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }


    public async Task<BaseResponse<PagedResponse<UserResponse>>> GetUserList(UserRequest request)
    {
        IQueryable<User> query = _repository.GetUserQuery();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();

            query = query.Where(u =>
                u.FirstName.ToLower().Contains(search) ||
                u.LastName.ToLower().Contains(search) ||
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
        List<UserResponse> records = new();

        foreach (var user in users)
        {
            records.Add(new UserResponse
            {
                UserId = user.UserId,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                IsDeleted = user.IsDeleted
            });
        }

        PagedResponse<UserResponse> response = new PagedResponse<UserResponse>()
        {
            Items = records,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
        };

        return ResponseFactory.Success(
            response,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }
}