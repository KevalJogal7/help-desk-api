
namespace HelpDesk.Services.DTOs.LoginDTOs;
public class LoginResponse
{
    public string AccessToken { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
}