
using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;

public sealed record GetSupportingDocsByUserIdQueryResponse(IReadOnlyList<SupportingDocModel> SupportingDocs);
