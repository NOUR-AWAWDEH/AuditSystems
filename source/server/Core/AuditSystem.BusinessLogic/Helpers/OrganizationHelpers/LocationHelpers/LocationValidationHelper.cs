using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;

public class LocationValidationHelper(IRepository<Guid, Location> repository) : ILocationValidation
{
    private readonly IRepository<Guid, Location> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(LocationConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<Location, Guid>(
            _repository.Query(),
            l => l.Id == locationId,
            LocationConstants.LocationOwnerIdSelector,
            userId,
            string.Format(LocationConstants.LocationNotFoundMessage, locationId),
            string.Format(LocationConstants.UnauthorizedAccessMessage, userId, locationId)
        );
    }
}