namespace HelpDesk.Services.DTOs.ProfileDTOs;

public class ProfileResponse
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; }
}
