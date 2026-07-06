
namespace HelpDesk.Services.DTOs;
public class LoginResponse
{
    public string AccessToken { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public int RoleId { get; set; }
    public DateTime Expiration { get; set; }
}