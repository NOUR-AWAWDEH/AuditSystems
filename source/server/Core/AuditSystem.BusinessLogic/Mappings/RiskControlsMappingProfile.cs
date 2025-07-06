using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;
using AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;
using AuditSystem.Application.Features.RiskControls.RiskControls.Create;
using AuditSystem.Application.Features.RiskControls.RiskControls.Update;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Create;
using AuditSystem.Application.Features.RiskControls.RiskProgram.Update;
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

        //RiskControls
        CreateMap<RiskControlsModel, RiskControls>().ReverseMap();
        CreateMap<CreateRiskControlsCommand, RiskControlsModel>();
        CreateMap<UpdateRiskControlsCommand, RiskControlsModel>();

        //RiskProgram
        CreateMap<RiskProgramModel, RiskProgram>().ReverseMap();
        CreateMap<CreateRiskProgramCommand, RiskProgramModel>();
        CreateMap<UpdateRiskProgramCommand, RiskProgramModel>();
    }
}
