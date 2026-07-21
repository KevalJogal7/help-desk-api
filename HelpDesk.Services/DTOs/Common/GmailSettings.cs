namespace HelpDesk.Services.DTOs.Common;

public class GmailSettings
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string ApplicationName { get; set; } = "HelpDesk";
}