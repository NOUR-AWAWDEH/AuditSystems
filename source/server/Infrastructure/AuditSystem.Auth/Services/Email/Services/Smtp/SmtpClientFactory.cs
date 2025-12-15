using System.Net;
using System.Net.Mail;
using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using Microsoft.Extensions.Options;

namespace AuditSystem.Auth.Services.Email.Services.Smtp;

public class SmtpClientFactory : ISmtpClientFactory
{
    private readonly SmtpSettings _settings;

    public SmtpClientFactory(IOptions<SmtpSettings> settings)
    {
        _settings = settings.Value;
    }

    public ISmtpClient CreateClient()
    {
        var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(_settings.SenderAddress, _settings.Password),
            EnableSsl = _settings.EnableSsl
        };

        return new SmtpClientWrapper(client);
    }
}
