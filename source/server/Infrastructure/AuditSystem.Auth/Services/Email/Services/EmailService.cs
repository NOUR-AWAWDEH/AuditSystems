using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using Microsoft.Extensions.Configuration;

namespace AuditSystem.Auth.Services.Email.Services;

public class EmailService : IEmailService
{
    private readonly IEmailTemplateRenderer _renderer;
    private readonly IEmailSender _sender;
    private readonly string _clientDomain;

    public EmailService(
        IEmailTemplateRenderer renderer,
        IEmailSender sender,
        IConfiguration configuration)
    {
        _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var origins = configuration["CORS_ALLOWED_ORIGIN"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        _clientDomain = origins?.FirstOrDefault() ?? "http://localhost:10000";
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
            supportEmail: "support@auditsystem.online"
        );

        await SendEmailAsync(request);
    }

    public async Task SendPasswordChangedConfirmationAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var request = new PasswordChangedEmailRequest(email, "support@auditsystem.online");
        await SendEmailAsync(request);
    }

    public async Task SendDriftReportNotificationToActionOwnerAsync(string actionOwnerEmail, string actionOwnerName, Guid driftReportId)
    {
        if (string.IsNullOrWhiteSpace(actionOwnerEmail))
            throw new ArgumentException("Action owner email cannot be empty", nameof(actionOwnerEmail));

        if (string.IsNullOrWhiteSpace(actionOwnerName))
            throw new ArgumentException("Action owner name cannot be empty", nameof(actionOwnerName));

        var placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "ActionOwnerName", new PlaceholderValue { Value = actionOwnerName } },
            { "DriftReportId", new PlaceholderValue { Value = driftReportId.ToString() } },
            { "DriftReportLink", new PlaceholderValue { Value = $"{_clientDomain}/reports/drift/{driftReportId}" } }
        };

        var request = new EmailRequest
        {
            ToEmails = new List<string> { actionOwnerEmail },
            Subject = "New Drift Report Notification",
            TemplateName = "DriftReportNotification.html",
            Placeholders = placeholders
        };

        await SendEmailAsync(request);
    }
}