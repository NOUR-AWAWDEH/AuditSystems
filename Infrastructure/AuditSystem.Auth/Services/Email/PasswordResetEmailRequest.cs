namespace AuditSystem.Auth.Services.Email;

public class PasswordResetEmailRequest : EmailRequest
{
    public override string TemplateName => "PasswordReset";
    public override string Subject => "Password Reset Code";

    public PasswordResetEmailRequest(string email, string userName, string resetCode, string supportEmail)
    {
        ToEmails.Add(email);
        Placeholders = new Dictionary<string, string>
        {
            { "ResetCode", resetCode },
            { "UserName", userName },
            { "SupportEmail", supportEmail }
        };
    }
}
