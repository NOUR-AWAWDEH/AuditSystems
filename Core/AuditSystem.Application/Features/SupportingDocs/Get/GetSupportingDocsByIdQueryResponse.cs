
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Application.Features.SupportingDocs.Get;

public sealed record class GetSupportingDocsByIdQueryResponse(SupportingDocModel SupportingDocsModel);