using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.Application.Constants.ObservationConst;

public static class ObservationConstants
{
    public static readonly string[] ObservationTags = ["observation", "observation-list"];
    public static readonly string[] ListTags = ["observation-list"];

    public static readonly Expression<Func<Observation, object?>>[] IncludeHierarchy =
    [
        o => o.AuditProgram,
        o => o.AuditProgram.ExitControl,
        o => o.AuditProgram.ExitControl.Risk,
        o => o.AuditProgram.ExitControl.Risk.Objective,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation,
        o => o.AuditProgram.ExitControl.Risk.Objective.RiskControlMatrix.SubProcess.AuditProcess.SubDepartment.Department.SubLocation.Location,
    ];

    public const string EntityName = "Observation";
    
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access observation {1}.";
    public const string UnauthorizedAuditProgramAccessMessage = "User {0} is not authorized to access audit program {1}.";
    public const string ObservationNotFoundMessage = "Observation with ID {0} not found.";
    public const string AuditProgramNotFoundMessage = "Audit program with ID {0} not found.";
    public const string UnauthorizedObservationAccessMessage = "User {0} is not authorized to access observation with id: {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved observations search results from cache";
}