using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Auth.Services.Email.Services.Renderers;

public sealed class EmbeddedEmailTemplateRenderer : IEmailTemplateRenderer
{
    private readonly ILogger<EmbeddedEmailTemplateRenderer> _logger;

    public EmbeddedEmailTemplateRenderer(
        ILogger<EmbeddedEmailTemplateRenderer> logger)
    {
        _logger = logger;
    }

    public string Render(
        string templateName,
        Dictionary<string, PlaceholderValue>? placeholders)
    {
        var template = LoadTemplate(templateName);
        return ApplyPlaceholders(templateName, template, placeholders);
    }

    private string LoadTemplate(string templateName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName =
            $"AuditSystem.Auth.Services.Email.EmailTemplates.{templateName}.html";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            _logger.LogError(
                "Email template not found: {ResourceName}", resourceName);
            throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    private string ApplyPlaceholders(
        string templateName,
        string template,
        Dictionary<string, PlaceholderValue>? placeholders)
    {
        if (placeholders != null && placeholders.Any())
        {
            foreach (var (key, value) in placeholders)
            {
                var resolved = value.IsHtmlContent
                    ? value.Value
                    : WebUtility.HtmlEncode(value.Value ?? string.Empty);

                template = template.Replace($"{{{{{key}}}}}", resolved);
            }
        }

        if (template.Contains("{{") && template.Contains("}}"))
        {
            _logger.LogWarning(
                "Unreplaced placeholders found in template {TemplateName}",
                templateName);
        }

        return template;
    }
}
