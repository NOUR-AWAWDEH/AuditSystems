using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.UserDesignation;

namespace AuditSystem.Application.Features.UserDesignation.Search;

public class SearchUserDesignationQueryValidator 
    : PaginationRequestValidator<UserDesignationModel, SearchUserDesignationQuery>;