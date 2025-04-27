using AuditSystem.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Behaviors;

internal sealed class MediatRLoggingBehavior<TRequest, TResponse>(
    ILogger<MediatRLoggingBehavior<TRequest,
    TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var commandName = request.GetGenericTypeName();

        logger.LogInformation("Handling command '{commandName}'", commandName);

        var response = await next();

        logger.LogInformation("Command '{commandName}' handled.", commandName);

        return response;
    }
}