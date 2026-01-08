using AuditSystem.Application.Constants.Audit.AuditProgramConst;
using AuditSystem.Application.Constants.Risks.ExitControlConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Helpers.AuditProgramHelpers;

public class AuditProgramValidationHelper(
    IRepository<Guid, AuditProgram> repository,
    IRepository<Guid, ExitControl> exitControlRepository,
    IRepository<Guid, AppUser> userRepository,
    ILogger<AuditProgramValidationHelper> logger)
    : IAuditProgramValidation
{
    private readonly IRepository<Guid, AuditProgram> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));

    private readonly IRepository<Guid, ExitControl> _exitControlRepository = exitControlRepository
        ?? throw new ArgumentNullException(nameof(exitControlRepository));

    private readonly IRepository<Guid, AppUser> _userRepository = userRepository
        ?? throw new ArgumentNullException(nameof(userRepository));

    private readonly ILogger<AuditProgramValidationHelper> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                AuditProgramConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<AuditProgram> ValidateAuditProgramOwnershipAsync(Guid auditProgramId, Guid userId)
    {
        _logger.LogDebug(
            "Validating ownership for audit program {AuditProgramId} for user {UserId}",
            auditProgramId, userId);

        var auditProgram = await _repository.GetAll()
            .Include(ap => ap.ExitControl)
                .ThenInclude(ec => ec.Objective)
                .ThenInclude(o => o.RiskControlMatrix)
                .ThenInclude(rcm => rcm.AuditProcess)
                .ThenInclude(ap => ap.SubDepartment)
                .ThenInclude(sd => sd.Department)
                .ThenInclude(d => d.Location)
            .AsSplitQuery()
            .FirstOrDefaultAsync(ap => ap.Id == auditProgramId);

        if (auditProgram == null)
        {
            _logger.LogWarning(
                "Audit program with ID {AuditProgramId} not found",
                auditProgramId);

            throw new KeyNotFoundException(
                string.Format(
                    AuditProgramConstants.AuditProgramNotFoundMessage,
                    auditProgramId));
        }

        return auditProgram;
    }

    public async Task<ExitControl> ValidateExitControlOwnershipAsync(Guid exitControlId, Guid userId)
    {
        var exitControl = await _exitControlRepository.GetByIdAsync(exitControlId)
            ?? throw new ArgumentException(
                string.Format(
                    ExitControlConstants.ExitControlNotFoundMessage,
                    nameof(exitControlId)));

        var ownerId = await _exitControlRepository.Query()
            .AsNoTracking()
            .Where(ec => ec.Id == exitControlId)
            .Select(ExitControlConstants.ExitControlOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new ArgumentException(
                string.Format(
                    ExitControlConstants.UnauthorizedAccessMessage,
                    userId, exitControlId));

        return exitControl;
    }

    public async Task ValidateActionOwnerAssignmentAsync(Guid actionOwnerId, Guid userId)
    {
        if (actionOwnerId == Guid.Empty)
            throw new ArgumentException(
                "Action Owner ID is invalid.", nameof(actionOwnerId));

        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User ID is invalid.", nameof(userId));

        var isValid = await _userRepository.GetAll()
            .AsNoTracking()
            .AnyAsync(u =>
                u.Id == actionOwnerId &&
                u.CreatedByUserId == userId);

        if (!isValid)
        {
            _logger.LogWarning(
                "Unauthorized ActionOwner assignment attempt. User {UserId} tried to assign {ActionOwnerId}",
                userId, actionOwnerId);

            throw new UnauthorizedAccessException(
                "You are not allowed to assign this Action Owner.");
        }
    }
}
