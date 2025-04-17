using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.UserDesignation.Delete;
internal class DeleteUserDesignationCommandHandler(
    IUserDesignationService userDesignationService,
    ILogger<DeleteUserDesignationCommandHandler> logger) :
    IRequestHandler<DeleteUserDesignationCommand, Result>
{
    public async Task<Result> Handle(DeleteUserDesignationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await userDesignationService.DeleteUserDesignationAsync(request.Id);
            logger.LogInformation("User Designation with id: {Id} was deleted", request.Id);
            return Result.Success();  
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting User Designation with id: {Id}", request.Id);
            return Result.Error("Error deleting User Designation");
        }  
    }
}
