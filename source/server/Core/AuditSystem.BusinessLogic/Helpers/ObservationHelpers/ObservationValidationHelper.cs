using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.BusinessLogic.Helpers.ObservationHelpers;

public class ObservationValidationHelper(
    IRepository<Guid, Observation> repository)
    : IObservationValidation
{
    private readonly IRepository<Guid, Observation> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                AuditProgramConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Observation> ValidateObservationOwnershipAsync(
        Guid observationId, Guid userId)
    {
        var observation = await _repository.GetByIdAsync(observationId, ObservationConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(
                ObservationConstants.ObservationNotFoundMessage,
                observationId
            ));

        if (observation.AuditProgram?.ExitControl?.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new UnauthorizedAccessException(string.Format(
                ObservationConstants.UnauthorizedObservationAccessMessage,
                userId, observationId
            ));

        return observation;
    }

    public async Task<AuditProgram> ValidateAuditProgramOwnershipAsync(Guid auditProgramId, Guid userId)
    {
        var auditProgram = await _repository.GetByIdAsync(auditProgramId, AuditProgramConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(
                AuditProgramConstants.AuditProgramNotFoundMessage,
                auditProgramId
            ));

        // Add null check for safety
        if (auditProgram.ExitControl?.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new UnauthorizedAccessException(string.Format(
                AuditProgramConstants.UnauthorizedAuditProgramAccessMessage,
                userId, auditProgramId
            ));
        
        return auditProgram;
    }
}