using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AuditSystem.Auth.Services.Email;

public class SmtpClientFactory : ISmtpClientFactory
{
    private readonly SmtpSettings _config;

    public SmtpClientFactory(IOptions<SmtpSettings> config)
    {
        _config = config.Value ?? throw new ArgumentNullException(nameof(config));
    }

    public ISmtpClient CreateClient()
    {
        var client = new SmtpClient
        {
            Host = _config.Host,
            Port = _config.Port,
            EnableSsl = _config.EnableSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_config.SenderAddress, _config.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        return new SmtpClientWrapper(client);
    }
}
