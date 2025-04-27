using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.UserDesignation;
using MediatR;

namespace AuditSystem.Application.Features.UserDesignation.Search;

internal sealed class SearchUserDesignationQueryHandler(IUserDesignationService userDesignationService)
 : IRequestHandler<SearchUserDesignationQuery, Result<PaginationRequestResponse<UserDesignationModel>>>
{
   public async Task<Result<PaginationRequestResponse<UserDesignationModel>>> Handle(SearchUserDesignationQuery request, CancellationToken cancellationToken)
   {
      var page = new Page(request.PageNumber, request.PageSize);
      var searchResult = await userDesignationService.SearchAsync(request.SearchTerm, page);
      var response = new PaginationRequestResponse<UserDesignationModel>(searchResult.Items, searchResult.TotalCount);
      return Result.Success(response);
   } 
}