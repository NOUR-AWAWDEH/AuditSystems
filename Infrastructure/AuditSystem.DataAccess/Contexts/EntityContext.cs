using AuditSystem.Contract.Interfaces.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using System.Transactions;

namespace AuditSystem.DataAccess.Contexts;

internal sealed class EntityContext(ApplicationDbContext context) : IContext
{
    private bool _disposed;
    private IDbContextTransaction? _dbContextTransaction;

    public async Task CommitTransactionAsync()
    {
        if (_dbContextTransaction is null) throw new TransactionException("Transaction has not been started");
        await _dbContextTransaction.CommitAsync();
    }

    public async Task RollBackAsync()
    {
        if (_dbContextTransaction is null) throw new TransactionException("Transaction has not been started");
        await _dbContextTransaction.RollbackAsync();
    }

    public async Task StartTransactionAsync()
    {
        _dbContextTransaction = await context.Database.BeginTransactionAsync();
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

        if (disposing && _dbContextTransaction is not null)
        {
            await _dbContextTransaction.DisposeAsync();
        }

        _disposed = true;
    }
}