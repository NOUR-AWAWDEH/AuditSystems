namespace AuditSystem.Auth.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string receptor, string subject, string body);
    }
}
