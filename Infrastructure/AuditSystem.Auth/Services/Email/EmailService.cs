using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace AuditSystem.Auth.Services.Email;

public class EmailService : IEmailService
{
    private readonly SMTPConfigModel _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SMTPConfigModel> smtpConfig, ILogger<EmailService> logger)
    {
        _config = smtpConfig.Value ?? throw new ArgumentNullException(nameof(smtpConfig));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendEmailAsync(EmailRequest emailRequest)
    {
        if (emailRequest == null)
            throw new ArgumentNullException(nameof(emailRequest));

        if (string.IsNullOrWhiteSpace(emailRequest.TemplateName))
            throw new ArgumentException("Template name cannot be empty", nameof(emailRequest.TemplateName));

        if (emailRequest.ToEmails == null || !emailRequest.ToEmails.Any())
            throw new ArgumentException("At least one recipient email is required", nameof(emailRequest.ToEmails));

        try
        {
            var body = GetEmailBody(emailRequest.TemplateName, emailRequest.Placeholders);
            if (string.IsNullOrWhiteSpace(body))
                throw new InvalidOperationException($"Email template '{emailRequest.TemplateName}' is empty");

            await SendEmailInternalAsync(
                emailRequest.ToEmails,
                emailRequest.Subject,
                body,
                isHtml: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(", ", emailRequest.ToEmails));
            throw;
        }
    }

    public async Task SendEmailAsync(string email, string subject, string body, bool isHtml = true)
    {
        await SendEmailAsync(new List<string> { email }, subject, body, isHtml);
    }

    public async Task SendEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true)
    {
        await SendEmailInternalAsync(recipients, subject, body, isHtml);
    }

    private async Task SendEmailInternalAsync(List<string> recipients, string subject, string body, bool isHtml)
    {
        if (recipients == null || recipients.Count == 0)
            throw new ArgumentException("Recipient email list cannot be empty.", nameof(recipients));

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_config.SenderAddress, _config.SenderDisplayName),
            IsBodyHtml = isHtml,
            Subject = subject,
            Body = body,
            BodyEncoding = Encoding.UTF8
        };

        foreach (var recipient in recipients.Where(r => !string.IsNullOrWhiteSpace(r)))
        {
            mailMessage.To.Add(recipient);
        }

        if (mailMessage.To.Count == 0)
            throw new ArgumentException("No valid email addresses provided.", nameof(recipients));

        using var smtpClient = new SmtpClient
        {
            Host = _config.Host,
            Port = _config.Port,
            EnableSsl = _config.EnableSSL,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_config.UserName, _config.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {Recipients}", string.Join(", ", recipients));
        }
        catch (SmtpException ex)
        {
            throw new EmailSendException("Failed to send email", ex);
        }
    }

    private string GetEmailBody(string templateName, Dictionary<string, PlaceholderValue> placeholders = null)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"AuditSystem.Auth.Services.Email.EmailTemplates.{templateName}.html";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            _logger.LogError("Email template not found: {ResourceName}", resourceName);
            throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        var template = reader.ReadToEnd();

        if (placeholders == null || !placeholders.Any())
            return template;

        foreach (var placeholder in placeholders)
        {
            var value = placeholder.Value.IsHtmlContent
                ? placeholder.Value.Value
                : WebUtility.HtmlEncode(placeholder.Value.Value ?? string.Empty);

            template = template.Replace($"{{{{{placeholder.Key}}}}}", value);

            if (template.Contains("{{") && template.Contains("}}"))
            {
                _logger.LogWarning("Unreplaced placeholders found in email template: {TemplateName}", templateName);
            }
        }

        return template;
    }



}