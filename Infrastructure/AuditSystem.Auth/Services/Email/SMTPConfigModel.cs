﻿namespace AuditSystem.Auth.Services.Email;

public class SMTPConfigModel
{
    public string SenderAddress { get; set; } = string.Empty;
    public string SenderDisplayName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSSL { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public bool IsBodyHtml { get; set; }
}
