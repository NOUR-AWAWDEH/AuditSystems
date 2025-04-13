using AuditSystem.Application.Features.Risks.Risk.Create;
using AuditSystem.Application.Features.Risks.Risk.Update;
using AuditSystem.Application.Features.Risks.RiskAssessment.Create;
using AuditSystem.Application.Features.Risks.RiskAssessment.Update;
using AuditSystem.Application.Features.Risks.RiskFactor.Create;
using AuditSystem.Application.Features.Risks.RiskFactor.Update;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;
 
public sealed class RisksMappingProfile : Profile
{
    public RisksMappingProfile()
    {
        //RiskAssessment
        CreateMap<RiskAssessmentModel, RiskAssessment>().ReverseMap();
        CreateMap<CreateRiskAssessmentCommand, RiskAssessmentModel>();
        CreateMap<UpdateRiskAssessmentCommand, RiskAssessmentModel>();

        //RiskAssessment
        CreateMap<RiskFactorModel, RiskFactor>().ReverseMap();
        CreateMap<CreateRiskFactorCommand, RiskFactorModel>();
        CreateMap<UpdateRiskFactorCommand, RiskFactorModel>();

        //Risk
        CreateMap<RiskModel, Risk>().ReverseMap();
        CreateMap<CreateRiskCommand, RiskModel>();
        CreateMap<UpdateRiskCommand, RiskModel>();
    }
}