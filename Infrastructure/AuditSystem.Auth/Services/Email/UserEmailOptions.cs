namespace AuditSystem.Auth.Services.Email;

public class UserEmailOptions
{
    public List<string> ToEmails { get; set; } = new List<string>();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
}