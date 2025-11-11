namespace AuditSystem.Contract.Interfaces.Contexts;

public interface IContext : IAsyncDisposable
{
    public Task StartTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollBackAsync();
}
