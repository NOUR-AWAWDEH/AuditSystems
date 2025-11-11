namespace AuditSystem.Host.Seed.Exceptions;

public sealed class SeedingException : Exception
{
    public SeedingException(string message) : base(message) { }
    public SeedingException(string message, Exception innerException) : base(message, innerException) { }
}