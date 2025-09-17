using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;

public class AuditProgramValidationHelper(
    IRepository<Guid, AuditProgram> repository)
    : IAuditProgramValidation
{
    private readonly IRepository<Guid, AuditProgram> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                AuditProgramConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<AuditProgram> ValidateAuditProgramOwnershipAsync(
        Guid auditProgramId, Guid userId)
    {
        var auditProgram = await _repository.GetByIdAsync(auditProgramId, AuditProgramConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(
                AuditProgramConstants.AuditProgramNotFoundMessage,
                auditProgramId
            ));

        if (auditProgram.ExitControl?.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new UnauthorizedAccessException(string.Format(
                AuditProgramConstants.UnauthorizedAuditProgramAccessMessage,
                userId, auditProgramId
            ));

        return auditProgram;
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {
        var exitControl = await _repository.GetByIdAsync(exitControlId, ExitControlConstants.IncludeHierarchy)
            ?? throw new KeyNotFoundException(string.Format(
                AuditProgramConstants.ExitControlNotFoundMessage,
                 exitControlId
            ));

        // Add null check for safety
        if (exitControl.Risk?.Objective?.RiskControlMatrix?.SubProcess?.AuditProcess?.SubDepartment?.Department?.SubLocation?.Location?.UserId != userId)
            throw new UnauthorizedAccessException(string.Format(
                AuditProgramConstants.UnauthorizedExitControlAccessMessage,
                userId, exitControlId
            ));
        
        return exitControl;
    }
}