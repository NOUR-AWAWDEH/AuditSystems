﻿namespace AuditSystem.Auth.Services.Email;

public class EmailSendException : Exception
{
    public EmailSendException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}