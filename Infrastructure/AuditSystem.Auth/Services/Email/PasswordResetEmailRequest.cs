namespace AuditSystem.Auth.Services.Email;

public class PasswordResetEmailRequest : EmailRequest
{
    public override string TemplateName => "PasswordReset";
    public override string Subject => "Password Reset Code";

    public PasswordResetEmailRequest(string email, string userName, string resetCode, string supportEmail)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(resetCode))
            throw new ArgumentException("Reset code cannot be empty", nameof(resetCode));

        ToEmails.Add(email);
        Placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "ResetCode", new PlaceholderValue { Value = resetCode, IsHtmlContent = false } },
            { "UserName", new PlaceholderValue { Value = userName, IsHtmlContent = false } },
            { "SupportEmail", new PlaceholderValue { Value = supportEmail, IsHtmlContent = false } }
        };
    }
}