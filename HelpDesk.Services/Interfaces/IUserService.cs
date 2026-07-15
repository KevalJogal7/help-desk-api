using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.UserDTOs;

namespace HelpDesk.Services.Interfaces;

public interface IUserService
{
    Task<BaseResponse<PagedResponse<UserResponse>>> GetUserList(UserRequest request);
}
