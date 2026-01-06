using AuditSystem.Application.Features.Reports.DriftReport.Create;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Org.BouncyCastle.Crypto.Agreement.Srp;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class DriftReportMappingProfile : Profile
{
    public DriftReportMappingProfile()
    {
        // Response ↔ Model
        CreateMap<CreateDriftReportCommandResponse, DriftReportModel>().ReverseMap();

        // Command ↔ Request
        CreateMap<CreateDriftReportCommand, GenerateDriftReportRequest>().ReverseMap();

        // Request → Entity
        CreateMap<GenerateDriftReportRequest, DriftReport>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // DB generated
            .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
            .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationId))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.SubDepartmentId, opt => opt.MapFrom(src => src.SubDepartmentId))
            .ForMember(dest => dest.ObjectiveId, opt => opt.MapFrom(src => src.ObjectiveId))
            .ForMember(dest => dest.AuditProgramId, opt => opt.MapFrom(src => src.AuditProgramId))
            .ForMember(dest => dest.ActionDate, opt => opt.MapFrom(src => src.ActionDate))
            .ForMember(dest => dest.ActionPlan, opt => opt.MapFrom(src => src.ActionPlan))
            .ForMember(dest => dest.IsAgreed, opt => opt.MapFrom(src => src.IsAgreed))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
    }
}
