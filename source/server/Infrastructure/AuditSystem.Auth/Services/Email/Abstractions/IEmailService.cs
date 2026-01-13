using AuditSystem.Auth.Services.Email.Models;

namespace AuditSystem.Auth.Services.Email.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest emailRequest);
    Task SendEmailAsync(string email, string subject, string body, bool isHtml = true);
    Task SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true);
    Task SendPasswordResetEmailAsync(string email, string resetLink);
    Task SendPasswordChangedConfirmationAsync(string email);
    Task SendDriftReportNotificationToActionOwnerAsync(string actionOwnerEmail, string actionOwnerName, Guid driftReportId);

}