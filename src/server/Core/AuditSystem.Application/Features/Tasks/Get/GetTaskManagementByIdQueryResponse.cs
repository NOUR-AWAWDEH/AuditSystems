using AuditSystem.Contract.Models.Tasks;

namespace AuditSystem.Application.Features.Tasks.Get;

public sealed record class GetTaskManagementByIdQueryResponse(TaskManagementModel TaskManagementModel);