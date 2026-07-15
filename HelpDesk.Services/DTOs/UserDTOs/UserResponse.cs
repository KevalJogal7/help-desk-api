namespace HelpDesk.Services.DTOs.UserDTOs;

public class UserResponse 
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int RoleId { get; set; }

    public string Role { get; set; } = null!;
}
