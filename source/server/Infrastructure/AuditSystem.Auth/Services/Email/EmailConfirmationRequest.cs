namespace AuditSystem.Auth.Services.Email;

public class EmailConfirmationRequest : EmailRequest
{
    public override string TemplateName => "EmailConfirmation";
    public override string Subject => "Confirm Your Email";

    public EmailConfirmationRequest(string email, string userName, string confirmationLink)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("UserName cannot be empty", nameof(userName));
        if (string.IsNullOrWhiteSpace(confirmationLink))
            throw new ArgumentException("ConfirmationLink cannot be empty", nameof(confirmationLink));
        if (!Uri.TryCreate(confirmationLink, UriKind.Absolute, out _))
            throw new ArgumentException("ConfirmationLink must be a valid URL", nameof(confirmationLink));

        ToEmails.Add(email);
        Placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "UserName", new PlaceholderValue { Value = userName, IsHtmlContent = false } },
            { "ConfirmationLink", new PlaceholderValue { Value = confirmationLink, IsHtmlContent = false } }
        };
    }
}