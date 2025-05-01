namespace AuditSystem.Contract.Interfaces.Transaction;

public interface ITransaction : IAsyncDisposable
{
    Task StartTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackAsync();
}