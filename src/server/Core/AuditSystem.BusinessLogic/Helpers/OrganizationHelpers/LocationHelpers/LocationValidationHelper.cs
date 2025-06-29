using AuditSystem.Application.Constants.Organization.Location;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.LocationHelpers;

public class LocationValidationHelper
{
    private readonly IRepository<Guid, Location> _repository;

    public LocationValidationHelper(IRepository<Guid, Location> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(LocationConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId)
    {
        var location = await _repository.GetByIdAsync(locationId)
            ?? throw new KeyNotFoundException(string.Format(LocationConstants.LocationNotFoundMessage, locationId));
        
        if (location.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(LocationConstants.UnauthorizedAccessMessage, userId, locationId));
        }
        
        return location;
    }
}