using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Application.Features.Common.Rating.Search;

public class SearchRatingQueryValidator
    : PaginationRequestValidator<RateModel, SearchRatingQuery>;