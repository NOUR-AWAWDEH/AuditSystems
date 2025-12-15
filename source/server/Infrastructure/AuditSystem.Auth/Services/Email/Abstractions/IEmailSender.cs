using AuditSystem.Auth.Services.Email.Models;

namespace AuditSystem.Auth.Services.Email.Abstractions;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message);
}
