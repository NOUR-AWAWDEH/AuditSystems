using AuditSystem.Application.Features.RiskControls.Objective.Create;
using AuditSystem.Application.Features.RiskControls.Objective.Update;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class RiskControlsMappingProfile : Profile
{
    public RiskControlsMappingProfile()
    {
        //RiskControlMatrix
        CreateMap<RiskControlMatrixModel, RiskControlMatrix>().ReverseMap();
        CreateMap<CreateRiskControlMatrixCommand, RiskControlMatrixModel>();
        CreateMap<UpdateRiskControlMatrixCommand, RiskControlMatrixModel>();

        //Objective
        CreateMap<ObjectiveModel, Objective>().ReverseMap();
        CreateMap<CreateObjectiveCommand, ObjectiveModel>();
        CreateMap<UpdateObjectiveCommand, ObjectiveModel>();
    }
}
