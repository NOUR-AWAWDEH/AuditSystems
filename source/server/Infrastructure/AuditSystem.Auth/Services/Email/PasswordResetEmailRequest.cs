namespace AuditSystem.Auth.Services.Email;

/// <summary>
/// Email request for password reset containing a secure reset link.
/// </summary>
public class PasswordResetEmailRequest : EmailRequest
{
    public override string TemplateName => "PasswordReset";
    public override string Subject => "Reset Your Password";

    public PasswordResetEmailRequest(string email, string userName, string resetUrl, string supportEmail)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(resetUrl))
            throw new ArgumentException("Reset URL cannot be empty", nameof(resetUrl));

        ToEmails.Add(email);
        Placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "ResetUrl", new PlaceholderValue { Value = resetUrl, IsHtmlContent = false } },
            { "UserName", new PlaceholderValue { Value = userName, IsHtmlContent = false } },
            { "SupportEmail", new PlaceholderValue { Value = supportEmail, IsHtmlContent = false } }
        };
    }
}