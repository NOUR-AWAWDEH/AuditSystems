namespace AuditSystem.Host.Exceptions;

public sealed class TransactionException(string message) : Exception(message);