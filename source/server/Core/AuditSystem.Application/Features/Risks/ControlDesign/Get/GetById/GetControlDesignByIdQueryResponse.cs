using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Get.GetById;

public sealed record GetControlDesignByIdQueryResponse(ControlDesignModel ControlDesignModel);