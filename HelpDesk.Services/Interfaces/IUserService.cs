using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.UserDTOs;

namespace HelpDesk.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponse<PagedResponse<UserResponse>>> GetUserList(UserRequest request);
    Task<BaseResponse<UserResponse>> GetUserById(Guid userId);
    Task<BaseResponse<object>> UpsertUser(UpsertUserRequest request);
    Task<BaseResponse<object>> ToggleUserStatus(Guid userId);
    Task<BaseResponse<object>> DeleteUser(Guid userId);
}
