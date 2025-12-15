using System.Net.Mail;

namespace AuditSystem.Auth.Services.Email.Abstractions;

public interface ISmtpClient : IDisposable
{
    Task SendMailAsync(MailMessage message);
}
