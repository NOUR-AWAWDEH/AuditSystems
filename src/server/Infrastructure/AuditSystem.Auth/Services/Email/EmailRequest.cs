namespace AuditSystem.Auth.Services.Email;

public class EmailRequest
{
    public List<string> ToEmails { get; set; } = new List<string>();
    public virtual string Subject { get; set; } = string.Empty; // Added string type
    public virtual string TemplateName { get; set; } = string.Empty; // Added string type
    public Dictionary<string, PlaceholderValue> Placeholders { get; set; } = new Dictionary<string, PlaceholderValue>();
}

