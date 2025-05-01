namespace AuditSystem.Auth.Services.Email;

public class NotificationEmailRequest : EmailRequest
{
    public override string TemplateName => "Notification";
    public override string Subject => "New Notification";

    public NotificationEmailRequest(string email, string userName, string message, string actionLink)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        ToEmails.Add(email);
        Placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "UserName", new PlaceholderValue { Value = userName, IsHtmlContent = false } },
            { "Message", new PlaceholderValue { Value = message, IsHtmlContent = false } },
            { "ActionLink", new PlaceholderValue { Value = actionLink, IsHtmlContent = false } }
        };
    }
}