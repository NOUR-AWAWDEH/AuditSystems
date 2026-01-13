namespace AuditSystem.Auth.Services.Security;

public class ClamAvSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 3310;
    public bool Enabled { get; set; } = false;
}
