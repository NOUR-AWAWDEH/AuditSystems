using System.Net.Mail;
using AuditSystem.Auth.Services.Email.Abstractions;

namespace AuditSystem.Auth.Services.Email.Services.Smtp;

public sealed class SmtpClientWrapper : ISmtpClient
{
    private readonly SmtpClient _client;

    public SmtpClientWrapper(SmtpClient client)
    {
        _client = client;
    }

    public Task SendMailAsync(MailMessage message)
    {
        return _client.SendMailAsync(message);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
