using Ardalis.Result;
using AuditSystem.Application.Base;
using MediatR;

namespace AuditSystem.Application.Features.Common.Rating.Delete;

public sealed record DeleteRateCommand(Guid Id) : ICommand<Result>;
