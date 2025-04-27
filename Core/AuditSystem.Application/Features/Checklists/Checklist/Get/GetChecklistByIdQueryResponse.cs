using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Application.Features.Checklists.Checklist.Get;

public sealed record class GetChecklistByIdQueryResponse(ChecklistModel ChecklistModel);
