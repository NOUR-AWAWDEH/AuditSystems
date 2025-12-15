using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace AuditSystem.Auth.Services.Email.Services;

public class EmailService : IEmailService
{
    private readonly IEmailTemplateRenderer _renderer;
    private readonly IEmailSender _sender;

    public EmailService(
        IEmailTemplateRenderer renderer,
        IEmailSender sender)
    {
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public async Task SendEmailAsync(EmailRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (string.IsNullOrWhiteSpace(request.TemplateName))
            throw new ArgumentException("Template name cannot be empty", nameof(request.TemplateName));

        if (string.IsNullOrWhiteSpace(request.Subject))
            throw new ArgumentException("Subject cannot be empty", nameof(request.Subject));

        if (request.ToEmails == null || !request.ToEmails.Any())
            throw new ArgumentException("At least one recipient email is required", nameof(request.ToEmails));

        var body = _renderer.Render(
            request.TemplateName,
            request.Placeholders);

        await _sender.SendAsync(new EmailMessage
        {
            To = request.ToEmails,
            Subject = request.Subject,
            Body = body
        });
    }

    public async Task SendEmailAsync(string email, string subject, string body, bool isHtml = true)
    {
        await SendEmailAsync(new List<string> { email }, subject, body, isHtml);
    }

    public async Task SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true)
    {
        if (recipients == null || !recipients.Any())
            throw new ArgumentException("Recipient email list cannot be empty.", nameof(recipients));

        await _sender.SendAsync(new EmailMessage
        {
            To = recipients,
            Subject = subject,
            Body = body,
            IsHtml = isHtml
        });
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetLink)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(resetLink))
            throw new ArgumentException("Reset link cannot be empty", nameof(resetLink));

        var request = new PasswordResetEmailRequest(
            email: email,
            userName: email.Split('@')[0],
            resetUrl: resetLink,
            supportEmail: "support@AuditSystem.com"
        );

        await SendEmailAsync(request);
    }

    public async Task SendPasswordChangedConfirmationAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var request = new PasswordChangedEmailRequest(email, "support@AuditSystem.online");
        await SendEmailAsync(request);
    }
}