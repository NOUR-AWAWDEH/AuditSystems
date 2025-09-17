using AuditSystem.Contract.Interfaces;
using MediatR;

namespace AuditSystem.Application.Base;

// Base class for user-aware commands
public abstract record UserAwareCommand<TResponse> : ICommand<TResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}

// Base class for user-aware queries
public abstract record UserAwareQuery<TResponse> : IRequest<TResponse>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}