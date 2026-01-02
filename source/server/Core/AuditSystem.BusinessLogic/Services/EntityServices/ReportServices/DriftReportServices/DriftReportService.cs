using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices;

public sealed class DriftReportService(
    IRepository<Guid, DriftReport> _repository,
    IOrganizationalDataFetcher _organizationalDataFetcher,
    IControlDataFetcher _controlDataFetcher,
    IRecommendationsFetcher _recommendationsFetcher,
    IMapper _mapper,
    ILogger<DriftReportService> _logger
) : IDriftReportService
{
    // -------------------
    // Create Drift Report
    // -------------------
    public async Task<DriftReportModel> CreateDriftReportAsync(GenerateDriftReportRequest command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));

        _logger.LogInformation(
            "Creating drift report for CompanyId: {CompanyId}, LocationId: {LocationId} by UserId: {UserId}",
            command.CompanyId, command.LocationId, command.UserId);

        // Map command → entity (persist IDs + basic info)
        var entity = _mapper.Map<DriftReport>(command);
        entity.CreatedByUserId = command.UserId;

        var createdEntity = await _repository.CreateAsync(entity);

        _logger.LogInformation(
            "Successfully created drift report {DriftReportId} for UserId: {UserId}",
            createdEntity.Id, command.UserId);

        // Return the full report with all related objects populated
        return await GenerateReportData(command);
    }

    // ----------------------
    // Generate Report Data
    // ----------------------
    public async Task<DriftReportModel> GenerateReportData(GenerateDriftReportRequest request)
    {
        _logger.LogInformation("Generating drift report data for CompanyId: {CompanyId}", request.CompanyId);

        // 1️⃣ Fetch all related organizational data
        var orgData = await _organizationalDataFetcher.FetchAsync(request);

        // 2️⃣ Fetch control data including risks & observations
        var controlData = await _controlDataFetcher.FetchAsync(request);

        // 3️⃣ Fetch recommendations for the observations
        var recommendations = await _recommendationsFetcher.FetchAsync(request, controlData.Observations);

        // 4️⃣ Build full report model
        var reportData = BuildDriftReportInfo(request, orgData, controlData, recommendations);

        _logger.LogInformation(
            "Drift report data generated successfully for CompanyId: {CompanyId}",
            request.CompanyId);

        return reportData;
    }

    // ----------------------
    // Build DriftReportModel
    // ----------------------
    private DriftReportModel BuildDriftReportInfo(
        GenerateDriftReportRequest request,
        OrganizationalData orgData,
        ControlData controlData,
        IReadOnlyList<RecommendationModel> recommendations)
    {
        _logger.LogDebug("Building drift report data");

        return new DriftReportModel
        {
            Company = _mapper.Map<CompanyModel>(orgData.Company),
            Location = _mapper.Map<LocationModel>(orgData.Location),
            Department = _mapper.Map<DepartmentModel>(orgData.Department),
            SubDepartment = _mapper.Map<SubDepartmentModel>(orgData.SubDepartment),
            Objective = _mapper.Map<ObjectiveModel>(controlData.Objective),
            AuditProgram = _mapper.Map<AuditProgramModel>(controlData.AuditProgram),
            Risks = _mapper.Map<IReadOnlyList<RiskModel>>(controlData.Risks),
            Observations = _mapper.Map<IReadOnlyList<ObservationModel>>(controlData.Observations),
            Recommendations = _mapper.Map<IReadOnlyList<RecommendationModel>>(recommendations),
            ActionDate = request.ActionDate,
            ActionPlan = request.ActionPlan,
            IsAgreed = request.IsAgreed,
            IsActive = request.IsActive,
            UserId = request.UserId
        };
    }

    // ----------------------
    // Optional: Get By Id
    // ----------------------
    public async Task<DriftReportModel?> GetDriftReportByIdAsync(Guid reportId)
    {
        _logger.LogDebug("Retrieving drift report {ReportId}", reportId);

        var entity = await _repository.GetByIdAsync(reportId);
        if (entity == null) return null;

        // Optionally build full report from fetchers
        var request = new GenerateDriftReportRequest
        {
            CompanyId = entity.CompanyId,
            LocationId = entity.LocationId,
            DepartmentId = entity.DepartmentId,
            SubDepartmentId = entity.SubDepartmentId,
            ObjectiveId = entity.ObjectiveId,
            AuditProgramId = entity.AuditProgramId,
            ActionDate = entity.ActionDate,
            ActionPlan = entity.ActionPlan,
            IsAgreed = entity.IsAgreed,
            IsActive = entity.IsActive,
            UserId = entity.CreatedByUserId
        };

        return await GenerateReportData(request);
    }
}
