using AuditSystem.Auth.Dtos;
namespace AuditSystem.Auth.Services.Email;
public interface IEmailService
{
    Task SendEmailAsync(EmailRequest emailRequest);
    Task SendEmailAsync(string email, string subject, string body, bool isHtml = true);
    Task SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true);
}