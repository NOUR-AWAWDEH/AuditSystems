using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Models.Reports.DriftReport;

/// <summary>
/// Represents the complete data structure for a Drift Report
/// </summary>
public class DriftReportData
{
    /// <summary>
    /// Company information
    /// </summary>
    public CompanyModel? Company { get; set; }

    /// <summary>
    /// Location information
    /// </summary>
    public LocationModel? Location { get; set; }

    /// <summary>
    /// Department information
    /// </summary>
    public DepartmentModel? Department { get; set; }

    /// <summary>
    /// Sub-Department information
    /// </summary>
    public SubDepartmentModel? SubDepartment { get; set; }

    /// <summary>
    /// Objectives information
    /// </summary>
    public ObjectiveModel? Objective { get; set; }

    /// <summary>
    /// Collection of risks
    /// </summary>
    public IEnumerable<RiskModel> Risks { get; set; } = new List<RiskModel>();

    /// <summary>
    /// Audit program information
    /// </summary>
    public AuditProgramModel? AuditProgram { get; set; }

    /// <summary>
    /// Collection of observations with their recommendations
    /// </summary>
    public IEnumerable<ObservationModel> Observations { get; set; } = new List<ObservationModel>();

    /// <summary>
    /// Collection of recommendations associated with observations
    /// </summary>
    public IEnumerable<RecommendationModel> Recommendations { get; set; } = new List<RecommendationModel>();
}
