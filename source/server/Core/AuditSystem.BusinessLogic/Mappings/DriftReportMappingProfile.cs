using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Recommendations;
using AuditSystem.Domain.Entities.RiskControls;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

/// <summary>
/// Mapper profile for Drift Report related mappings
/// </summary>
public sealed class DriftReportMappingProfile : Profile
{
    public DriftReportMappingProfile()
    {
        // Company mapping
        CreateMap<Company, CompanyModel>();

        // Location mapping
        CreateMap<Location, LocationModel>();

        // Department mapping
        CreateMap<Department, DepartmentModel>();

        // SubDepartment mapping
        CreateMap<SubDepartment, SubDepartmentModel>();

        // Objective mapping
        CreateMap<Objective, ObjectiveModel>();

        // Risk mapping
        CreateMap<Risk, RiskModel>();

        // Audit Program mapping
        CreateMap<AuditProgram, AuditProgramModel>();

        // Recommendation mapping
        CreateMap<Recommendation, RecommendationModel>();

        // Observation mapping
        CreateMap<Observation, ObservationModel>();
    }
}
