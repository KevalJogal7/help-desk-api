using HelpDesk.Services.DTOs.Common;

namespace HelpDesk.Services.DTOs.UserDTOs;

public class UserRequest : PagedRequest
{
    public int Role { get; set; } = 0;
}
