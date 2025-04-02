namespace AuditSystem.Auth.Services.Email;

public abstract class EmailRequest
{
    public List<string> ToEmails { get; set; } = new();
    public Dictionary<string, string> Placeholders { get; set; } = new();
    public abstract string TemplateName { get; }
    public abstract string Subject { get; }
}
