namespace AuditSystem.Auth.Services.Email.Models;

/// <summary>
/// Email request for password change confirmation.
/// </summary>
public class PasswordChangedEmailRequest : EmailRequest
{
    public override string TemplateName => "PasswordChanged";
    public override string Subject => "Your Password Has Been Changed";

    public PasswordChangedEmailRequest(string email, string supportEmail)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        ToEmails.Add(email);

        // Extract username from email as fallback if needed, similar to PasswordResetEmailRequest
        var userName = email.Split('@')[0];

        Placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "UserName", new PlaceholderValue { Value = userName, IsHtmlContent = false } },
            { "SupportEmail", new PlaceholderValue { Value = supportEmail, IsHtmlContent = false } }
        };
    }
}
