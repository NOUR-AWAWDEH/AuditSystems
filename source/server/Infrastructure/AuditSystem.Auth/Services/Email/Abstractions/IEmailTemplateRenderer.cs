using AuditSystem.Auth.Services.Email.Models;

namespace AuditSystem.Auth.Services.Email.Abstractions;

public interface IEmailTemplateRenderer
{
    string Render(string templateName, Dictionary<string, PlaceholderValue>? placeholders);
}
