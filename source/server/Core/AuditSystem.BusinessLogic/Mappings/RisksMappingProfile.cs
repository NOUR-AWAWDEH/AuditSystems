using AuditSystem.Application.Features.Risks.ControlDesign.Create;
using AuditSystem.Application.Features.Risks.ControlDesign.Update;
using AuditSystem.Application.Features.Risks.ExitControl.Create;
using AuditSystem.Application.Features.Risks.ExitControl.Update;
using AuditSystem.Application.Features.Risks.Risk.Create;
using AuditSystem.Application.Features.Risks.Risk.Update;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class RisksMappingProfile : Profile
{
    public RisksMappingProfile()
    {
        //Risk
        CreateMap<RiskModel, Risk>().ReverseMap();
        CreateMap<CreateRiskCommand, RiskModel>();
        CreateMap<UpdateRiskCommand, RiskModel>();

        //ExitControl
        CreateMap<ExitControlModel, ExitControl>().ReverseMap();
        CreateMap<CreateExitControlCommand, ExitControlModel>();
        CreateMap<UpdateExitControlCommand, ExitControlModel>();

        //ControlDesign
        CreateMap<ControlDesignModel, ControlDesign>().ReverseMap();
        CreateMap<CreateControlDesignCommand, ControlDesignModel>();
        CreateMap<UpdateControlDesignCommand, ControlDesignModel>();
    }
}