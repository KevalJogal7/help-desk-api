namespace HelpDesk.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(
        string to,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default);

    Task SendEmailAsync(
        IEnumerable<string> recipients,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default);
}
