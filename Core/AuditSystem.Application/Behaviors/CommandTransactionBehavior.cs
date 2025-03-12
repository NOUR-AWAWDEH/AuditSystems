using AuditSystem.Application.Base;
using AuditSystem.Contract.Interfaces.Transaction;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Behaviors;

internal sealed class CommandTransactionBehavior<TRequest, TResponse>(
    ITransaction unitOfWork,
    ILogger<CommandTransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.StartTransactionAsync();
            logger.LogInformation("Transaction started for {CommandName}", typeof(TRequest).Name);
            var response = await next();
            await unitOfWork.CommitTransactionAsync();
            logger.LogInformation("Transaction committed for {CommandName}", typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            logger.LogError(ex, "Error during transaction for {CommandName}. Rollback changes.", typeof(TRequest).Name);
            throw;
        }
        finally
        {
            await unitOfWork.DisposeAsync();
        }
    }
}