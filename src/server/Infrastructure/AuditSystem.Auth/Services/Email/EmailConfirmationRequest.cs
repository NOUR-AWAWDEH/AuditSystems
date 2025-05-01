namespace AuditSystem.Auth.Services.Email;

public class EmailConfirmationRequest : EmailRequest
{
    public override string TemplateName => "EmailConfirmation";
    public override string Subject => "Confirm Your Email";

    public EmailConfirmationRequest(string email, string userName, string confirmationLink)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        ToEmails.Add(email);
        Placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "UserName", new PlaceholderValue { Value = userName, IsHtmlContent = false } },
            { "ConfirmationLink", new PlaceholderValue { Value = confirmationLink, IsHtmlContent = false } }
        };
    }
}