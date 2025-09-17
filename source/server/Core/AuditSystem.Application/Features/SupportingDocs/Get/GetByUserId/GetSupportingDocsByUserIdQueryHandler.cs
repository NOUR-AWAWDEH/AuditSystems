using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;

internal sealed class GetSupportingDocsByUserIdQueryHandler(
    ISupportingDocService supportingDocService,
    ILogger<GetSupportingDocsByUserIdQueryHandler> logger) 
    : IRequestHandler<GetSupportingDocsByUserIdQuery, Result<GetSupportingDocsByUserIdQueryResponse>>
{
    public async Task<Result<GetSupportingDocsByUserIdQueryResponse>> Handle(GetSupportingDocsByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await supportingDocService.GetSupportingDocsByUserIdAsync(request.UserId);
            return Result.Success(new GetSupportingDocsByUserIdQueryResponse(result), "Documents retrieved successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "No supporting documents found for user ID: {UserId}", request.UserId);
            return Result.NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving supporting documents for user ID: {UserId}", request.UserId);
            return Result.Error("Failed to retrieve supporting documents.");
        }
    }
}
