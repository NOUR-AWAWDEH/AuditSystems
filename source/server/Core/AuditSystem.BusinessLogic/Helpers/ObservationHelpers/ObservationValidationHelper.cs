using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Observations;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.ObservationHelpers;

public class ObservationValidationHelper(
    IRepository<Guid, Observation> repository,
    IRepository<Guid, AuditProgram> auditProgramRepository)
    : IObservationValidation
{
    private readonly IRepository<Guid, Observation> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, AuditProgram> _auditProgramRepository = auditProgramRepository
        ?? throw new ArgumentNullException(nameof(auditProgramRepository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                AuditProgramConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Observation> ValidateObservationOwnershipAsync(Guid observationId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<Observation, Guid>(
            _repository.Query(),
            o => o.Id == observationId,
            ObservationConstants.ObservationOwnerIdSelector,
            userId,
            string.Format(ObservationConstants.ObservationNotFoundMessage, observationId),
            string.Format(ObservationConstants.UnauthorizedObservationAccessMessage, userId, observationId)
        );
    }

    public async Task<AuditProgram> ValidateAuditProgramOwnershipAsync(Guid auditProgramId, Guid userId)
    {
        var auditProgram = await _auditProgramRepository.GetByIdAsync(auditProgramId)
             ?? throw new ArgumentException(AuditProgramConstants.AuditProgramNotFoundMessage, nameof(auditProgramId));

        var ownerId = await _auditProgramRepository.Query()
            .AsNoTracking()
            .Where(a => a.Id == auditProgramId)
            .Select(AuditProgramConstants.AuditProgramOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(AuditProgramConstants.UnauthorizedAuditProgramAccessMessage, userId, auditProgramId)
            );

        return auditProgram;
    }
}