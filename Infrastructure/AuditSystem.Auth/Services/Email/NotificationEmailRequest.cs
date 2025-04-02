namespace AuditSystem.Auth.Services.Email;

public class NotificationEmailRequest : EmailRequest
{
    public override string TemplateName => "Notification";
    public override string Subject => "New Notification";

    public NotificationEmailRequest(string email, string userName, string message, string actionLink)
    {
        ToEmails.Add(email);
        Placeholders = new Dictionary<string, string>
        {
            { "UserName", userName },
            { "Message", message },
            { "ActionLink", actionLink }
        };
    }
}
