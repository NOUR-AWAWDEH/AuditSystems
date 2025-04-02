namespace AuditSystem.Auth.Services.Email;

public class EmailConfirmationRequest : EmailRequest
{
    public override string TemplateName => "EmailConfirmation";
    public override string Subject => "Confirm Your Email";

    public EmailConfirmationRequest(string email, string userName, string confirmationLink)
    {
        ToEmails.Add(email);
        Placeholders = new Dictionary<string, string>
        {
            { "UserName", userName },
            { "ConfirmationLink", confirmationLink }
        };
    }
}
