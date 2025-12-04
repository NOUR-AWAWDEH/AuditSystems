using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.JobTitle.Get.GetById;

public sealed record class GetJobTitleByIdQueryResponse(JobTitleModel JobTitleModel);