using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Application.Features.Checklists.Remark.Get;

public sealed record class GetRemarkByIdQueryResponse(RemarkModel RemarkModel);