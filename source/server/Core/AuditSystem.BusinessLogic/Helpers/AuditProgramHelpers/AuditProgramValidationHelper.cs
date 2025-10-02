using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.BusinessLogic.Utilities;
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

    public async Task<AuditProgram> ValidateAuditProgramOwnershipAsync(Guid auditProgramId, Guid userId)
    {
         return await OwnershipValidator.ValidateOwnershipAsync<AuditProgram, Guid>(
            _repository.Query(),
            ap => ap.Id == auditProgramId,
            AuditProgramConstants.AuditProgramOwnerIdSelector,
            userId,
            string.Format(AuditProgramConstants.AuditProgramNotFoundMessage, auditProgramId),
            string.Format(AuditProgramConstants.UnauthorizedAccessMessage, userId, auditProgramId)
         );
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {
       var exitControlWithRisk = await OwnershipValidator.ValidateOwnershipAsync<AuditProgram, Guid>(
            _repository.Query(),
            ap => ap.ExitControl.Id == exitControlId,
            AuditProgramConstants.AuditProgramOwnerIdSelector,
            userId,
            string.Format(ExitControlConstants.ExitControlNotFoundMessage, exitControlId),
            string.Format(ExitControlConstants.UnauthorizedAccessMessage, userId, exitControlId)
        );

        return exitControlWithRisk.ExitControl;
    }
}