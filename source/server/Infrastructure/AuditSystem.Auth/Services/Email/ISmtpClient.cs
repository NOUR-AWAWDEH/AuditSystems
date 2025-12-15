using System.Net.Mail;

namespace AuditSystem.Auth.Services.Email;

public interface ISmtpClient : IDisposable
{
    Task SendMailAsync(MailMessage message);
}
