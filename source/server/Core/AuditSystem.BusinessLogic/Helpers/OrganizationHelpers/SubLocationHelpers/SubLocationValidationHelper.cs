using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;

public class SubLocationValidationHelper(
    IRepository<Guid, SubLocation> subLocationRepository,
    IRepository<Guid, Location> locationRepository) 
    : ISubLocationValidation
{
    private readonly IRepository<Guid, SubLocation> _subLocationRepository = subLocationRepository ?? throw new ArgumentNullException(nameof(subLocationRepository));
    private readonly IRepository<Guid, Location> _locationRepository = locationRepository ?? throw new ArgumentNullException(nameof(locationRepository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SubLocationConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<Location, Guid>(
            _locationRepository.Query(),
            l => l.Id == locationId,
            LocationConstants.LocationOwnerIdSelector,
            userId,
            string.Format(LocationConstants.LocationNotFoundMessage, locationId),
            string.Format(LocationConstants.UnauthorizedLocationAccessMessage, userId, locationId)
        );
    }

    // Optimized: use preloaded entity to avoid extra query
    public Task<SubLocation> ValidateSubLocationOwnershipAsync(SubLocation subLocation, Guid userId)
    {
        if (subLocation == null)
            throw new KeyNotFoundException(string.Format(SubLocationConstants.SubLocationNotFoundMessage, Guid.Empty));

        // Validate location ownership first
        if (subLocation.Location == null)
            throw new KeyNotFoundException(string.Format(LocationConstants.LocationNotFoundMessage, subLocation.LocationId));

        if (subLocation.Location.UserId != userId)
            throw new UnauthorizedAccessException(
                string.Format(SubLocationConstants.UnauthorizedAccessMessage, userId, subLocation.Id)
            );

        return Task.FromResult(subLocation);
    }
}