using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Observation.Search;

internal sealed class SearchObservationQueryHandler(
    IObservationService observationService,
    ILogger<SearchObservationQueryHandler> logger)
    : IRequestHandler<SearchObservationQuery, Result<PaginationRequestResponse<ObservationModel>>>
{
    public async Task<Result<PaginationRequestResponse<ObservationModel>>> Handle(SearchObservationQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await observationService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.AuditProgramId, page);

        logger.LogInformation("Observations has been retrieved successfully by user with id: {UserId}", request.UserId);
        var response = new PaginationRequestResponse<ObservationModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}