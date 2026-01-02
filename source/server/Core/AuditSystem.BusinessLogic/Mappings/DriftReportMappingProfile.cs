using AuditSystem.Application.Features.Reports.DriftReport.Create;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

/// <summary>
/// Mapper profile for Drift Report related mappings
/// </summary>
public sealed class DriftReportMappingProfile : Profile
{
    public DriftReportMappingProfile()
    {
        // For handler response
        CreateMap<CreateDriftReportCommandResponse, DriftReportModel>().ReverseMap();

        // For service call
        CreateMap<CreateDriftReportCommand, GenerateDriftReportRequest>().ReverseMap();

        // Optional: command → entity if needed
        CreateMap<CreateDriftReportCommand, DriftReport>();
    }
}