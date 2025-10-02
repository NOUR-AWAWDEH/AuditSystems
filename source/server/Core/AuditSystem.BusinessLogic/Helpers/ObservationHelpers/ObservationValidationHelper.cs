using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.ObservationConst;
using AuditSystem.BusinessLogic.Utilities;
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
        var auditProgramWithObservation = await OwnershipValidator.ValidateOwnershipAsync<Observation, Guid>(
            _repository.Query(),
            o => o.AuditProgram.Id == auditProgramId,
            ObservationConstants.ObservationOwnerIdSelector,
            userId,
            string.Format(ObservationConstants.ObservationNotFoundMessage, auditProgramId),
            string.Format(ObservationConstants.UnauthorizedObservationAccessMessage, userId, auditProgramId)
        );

        return auditProgramWithObservation.AuditProgram;
    }
}