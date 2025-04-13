using AuditSystem.Application.Features.Reports.AuditExceptionReport.Create;
using AuditSystem.Application.Features.Reports.AuditExceptionReport.Update;
using AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Create;
using AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Update;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;
using AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Update;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;
using AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Update;
using AuditSystem.Application.Features.Reports.PlanningReport.Create;
using AuditSystem.Application.Features.Reports.PlanningReport.Update;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class ReportsMappingProfile : Profile
{
    public ReportsMappingProfile()
    {
        //AuditExceptionReport
        CreateMap<AuditExceptionReportModel, AuditExceptionReport>().ReverseMap();
        CreateMap<CreateAuditExceptionReportCommand, AuditExceptionReportModel>();
        CreateMap<UpdateAuditExceptionReportCommand, AuditExceptionReportModel>();

        //AuditPlanSummaryReport
        CreateMap<AuditPlanSummaryReportModel, AuditPlanSummaryReport>().ReverseMap();
        CreateMap<CreateAuditPlanSummaryReportCommand, AuditPlanSummaryReportModel>();
        CreateMap<UpdateAuditPlanSummaryReportCommand, AuditPlanSummaryReportModel>();

        //InternalAuditConsolidationReport
        CreateMap<InternalAuditConsolidationReportModel, InternalAuditConsolidationReport>().ReverseMap();
        CreateMap<CreateInternalAuditConsolidationReportCommand, InternalAuditConsolidationReportModel>();
        CreateMap<UpdateInternalAuditConsolidationReportCommand, InternalAuditConsolidationReportModel>();

        //JobTimeAllocationReport
        CreateMap<JobTimeAllocationReportModel, JobTimeAllocationReport>().ReverseMap();
        CreateMap<CreateJobTimeAllocationReportCommand, JobTimeAllocationReportModel>();
        CreateMap<UpdateJobTimeAllocationReportCommand, JobTimeAllocationReportModel>();

        //PlanningReport
        CreateMap<PlanningReportModel, PlanningReport>().ReverseMap();
        CreateMap<CreatePlanningReportCommand, PlanningReportModel>();
        CreateMap<UpdatePlanningReportCommand, PlanningReportModel>();

        //ReportingFollowUp
        CreateMap<ReportingFollowUpModel, ReportingFollowUp>().ReverseMap();
        CreateMap<CreatePlanningReportCommand, ReportingFollowUpModel>();
        CreateMap<UpdatePlanningReportCommand, ReportingFollowUpModel>();
    }
}
