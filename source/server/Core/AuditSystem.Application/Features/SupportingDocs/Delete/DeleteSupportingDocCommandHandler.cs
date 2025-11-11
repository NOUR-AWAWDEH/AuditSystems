using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using Microsoft.Extensions.Logging;
using Ardalis.Result;

namespace AuditSystem.Application.Features.SupportingDocs.Delete;
internal sealed class DeleteSupportingDocCommandHandler(
    ISupportingDocService supportingDocService,
    ILogger<DeleteSupportingDocCommandHandler> logger) :
    IRequestHandler<DeleteSupportingDocCommand, Result>
{
    public async Task<Result> Handle(DeleteSupportingDocCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await supportingDocService.DeleteSupportingDocAsync(request.Id, request.UserId);
            logger.LogInformation("Supporting doc with id: {Id} was deleted for user {UserId}", request.Id, request.UserId);
            return Result.SuccessWithMessage("Supporting document deleted successfully.");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while deleting Supporting Document with ID {Id} for user {UserId}.", request.Id, request.UserId);
            return Result.Error(ex.Message);
        }
    }
}
