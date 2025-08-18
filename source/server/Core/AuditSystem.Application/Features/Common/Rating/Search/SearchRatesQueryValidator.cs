using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Common.Rating.Search;

public class SearchRatesQueryValidator
    : PaginationRequestValidator<RateModel, SearchRatesQuery>;
