using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Application.Features.SupportingDocs.Search;

public class SearchSupportingDocsQueryValidator : PaginationRequestValidator<SupportingDocModel, SearchSupportingDocsQuery>;