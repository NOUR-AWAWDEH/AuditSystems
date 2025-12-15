namespace AuditSystem.Auth.Services.Email;

public interface ISmtpClientFactory
{
    ISmtpClient CreateClient();
}
