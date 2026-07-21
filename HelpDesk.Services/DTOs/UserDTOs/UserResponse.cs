using HelpDesk.Services.DTOs.ProfileDTOs;

namespace HelpDesk.Services.DTOs.UserDTOs;

public class UserResponse : ProfileResponse
{
    public bool IsDeleted { get; set; }

    public int RoleId { get; set; }

}
