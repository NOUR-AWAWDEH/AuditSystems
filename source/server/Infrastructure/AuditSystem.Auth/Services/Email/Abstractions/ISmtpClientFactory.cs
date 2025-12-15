namespace AuditSystem.Auth.Services.Email.Abstractions;

public interface ISmtpClientFactory
{
    ISmtpClient CreateClient();
}
