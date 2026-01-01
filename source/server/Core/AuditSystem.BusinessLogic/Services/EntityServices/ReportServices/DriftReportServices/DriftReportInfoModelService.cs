using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices;

public sealed class DriftReportInfoService(
    IOrganizationalDataFetcher organizationalDataFetcher,
    IControlDataFetcher controlDataFetcher,
    IRecommendationsFetcher recommendationsFetcher,
    IMapper mapper,
    ILogger<DriftReportInfoService> logger
) : IDriftReportInfoService
{
    private readonly IOrganizationalDataFetcher _organizationalDataFetcher = organizationalDataFetcher;
    private readonly IControlDataFetcher _controlDataFetcher = controlDataFetcher;
    private readonly IRecommendationsFetcher _recommendationsFetcher = recommendationsFetcher;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<DriftReportInfoService> _logger = logger;

    public async Task<DriftReportInfoModel> GenerateReportData(GenerateDriftReportRequest request)
    {
        _logger.LogInformation("Starting drift report data generation for CompanyId: {CompanyId}", request.CompanyId);

        // Step 1: Fetch organizational data
        var orgData = await _organizationalDataFetcher.FetchAsync(request);

        // Step 2: Fetch control data
        var controlData = await _controlDataFetcher.FetchAsync(request);

        // Step 3: Fetch recommendations
        var recommendations = await _recommendationsFetcher.FetchAsync(request, controlData.Observations);

        // Step 4: Build the report
        var reportData = BuildDriftReportInfo(orgData, controlData, recommendations);

        _logger.LogInformation("Drift report data generated successfully for CompanyId: {CompanyId}", request.CompanyId);

        return reportData;
    }


    private DriftReportInfoModel BuildDriftReportInfo(
        OrganizationalData orgData,
        ControlData controlData,
        IReadOnlyList<RecommendationModel> recommendations)
    {
        _logger.LogDebug("Building drift report data");

        return new DriftReportInfoModel
        {
            Company = _mapper.Map<CompanyModel>(orgData.Company),
            Location = _mapper.Map<LocationModel>(orgData.Location),
            Department = _mapper.Map<DepartmentModel>(orgData.Department),
            SubDepartment = _mapper.Map<SubDepartmentModel>(orgData.SubDepartment),
            Objective = _mapper.Map<ObjectiveModel>(controlData.Objective),
            AuditProgram = _mapper.Map<AuditProgramModel>(controlData.AuditProgram),
            Risks = _mapper.Map<IReadOnlyList<RiskModel>>(controlData.Risks),
            Observations = _mapper.Map<IReadOnlyList<ObservationModel>>(controlData.Observations),
            Recommendations = _mapper.Map<IReadOnlyList<RecommendationModel>>(recommendations)
        };
    }
}