using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubLocation.Delete;

internal sealed class DeleteSubLocationCommandHandler(
    ISubLocationService subLocationService,
    ILogger<DeleteSubLocationCommandHandler> logger) :
    IRequestHandler<DeleteSubLocationCommand, Result>
{
    public async Task<Result> Handle(DeleteSubLocationCommand request, CancellationToken cancellationToken)
    {
        await subLocationService.DeleteSubLocationAsync(request.Id, request.UserId);
        return Result.Success();
    }
}

