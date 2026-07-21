namespace HelpDesk.Services.Services;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

public class GmailEmailService : IEmailService
{
    private readonly GmailService _gmailService;
    private readonly GmailSettings _settings;

    public GmailEmailService(IOptions<GmailSettings> options)
    {
        _settings = options.Value;

        var token = new Google.Apis.Auth.OAuth2.Responses.TokenResponse
        {
            RefreshToken = _settings.RefreshToken
        };

        var credential = new UserCredential(
            new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _settings.ClientId,
                        ClientSecret = _settings.ClientSecret
                    },
                    Scopes = new[]
                    {
                        GmailService.Scope.GmailSend
                    }
                }),
            "user",
            token);

        _gmailService = new GmailService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _settings.ApplicationName
            });
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress("Help Desk", _settings.FromEmail));

        email.To.Add(MailboxAddress.Parse(to));

        email.Subject = subject;

        email.Body = new TextPart("html")
        {
            Text = body
        };

        using var memory = new MemoryStream();

        await email.WriteToAsync(memory);

        var raw = Convert.ToBase64String(memory.ToArray())
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        var message = new Google.Apis.Gmail.v1.Data.Message
        {
            Raw = raw
        };

        await _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();
    }
}    