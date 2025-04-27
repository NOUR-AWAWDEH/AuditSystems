using AuditSystem.Contract.Interfaces.Contexts;
using AuditSystem.Contract.Interfaces.Transaction;

namespace AuditSystem.BusinessLogic.Services.Transaction;

internal sealed class Transaction(IContext context) : ITransaction
{
    private bool _disposed;

    public async Task CommitTransactionAsync()
    {
        await context.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await context.RollBackAsync();
    }

    public async Task StartTransactionAsync()
    {
        await context.StartTransactionAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeObject(true);
        GC.SuppressFinalize(this);
    }

    private async ValueTask DisposeObject(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            await context.DisposeAsync();
        }

        _disposed = true;
    }
}