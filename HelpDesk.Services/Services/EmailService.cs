namespace HelpDesk.Services.Services;

using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default)
    {
        await SendEmailAsync(new[] { to }, subject, htmlBody, cancellationToken);
    }

    public async Task SendEmailAsync(
        IEnumerable<string> recipients,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();

        email.From.Add(
            new MailboxAddress(
                _settings.FromName,
                _settings.FromEmail));

        foreach (var recipient in recipients)
        {
            email.To.Add(MailboxAddress.Parse(recipient));
        }

        email.Subject = subject;

        email.Body = new BodyBuilder
        {
            HtmlBody = htmlBody
        }.ToMessageBody();

        using var logger = new MailKit.ProtocolLogger(Console.OpenStandardOutput());
        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            SecureSocketOptions.StartTls,
            cancellationToken);

        await smtp.AuthenticateAsync(
            _settings.Username,
            _settings.Password,
            cancellationToken);

        await smtp.SendAsync(email, cancellationToken);

        await smtp.DisconnectAsync(true, cancellationToken);
    }
}