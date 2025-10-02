using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;

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

    public async Task<SubLocation> ValidateSubLocationOwnershipAsync(Guid subLocationId, Guid userId)
    {
       return await OwnershipValidator.ValidateOwnershipAsync<SubLocation, Guid>(
            _subLocationRepository.Query(),
            sl => sl.Id == subLocationId,
            SubLocationConstants.SubLocationOwnerIdSelector,
            userId,
            string.Format(SubLocationConstants.SubLocationNotFoundMessage, subLocationId),
            string.Format(SubLocationConstants.UnauthorizedAccessMessage, userId, subLocationId)
        );
    }

    public async Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId)
    {
        var location = await _locationRepository.GetByIdAsync(locationId) 
            ?? throw new ArgumentException(LocationConstants.LocationNotFoundMessage, nameof(locationId));
        
        var ownerId = await _locationRepository.Query()
            .AsNoTracking()
            .Where(l => l.Id == locationId)
            .Select(LocationConstants.LocationOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(LocationConstants.UnauthorizedLocationAccessMessage, userId, locationId)
            );
        
        return location;
    }
}