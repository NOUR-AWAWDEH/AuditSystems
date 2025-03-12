using MediatR;

namespace AuditSystem.Application.Base;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}