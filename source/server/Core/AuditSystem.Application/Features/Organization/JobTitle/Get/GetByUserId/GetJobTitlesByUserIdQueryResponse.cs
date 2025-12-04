using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetByUserId;

public sealed record GetJobTitlesByUserIdQueryResponse(IReadOnlyList<JobTitleModel> JobTitles);
