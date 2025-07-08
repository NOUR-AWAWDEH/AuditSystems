using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.SubLocationHelpers;

public class SubLocationValidationHelper
{
    private readonly IRepository<Guid, SubLocation> _repository;

    public SubLocationValidationHelper(IRepository<Guid, SubLocation> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SubLocationConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<SubLocation> ValidateSubLocationOwnershipAsync(Guid subLocationId, Guid userId)
    {
        var subLocation = await _repository.GetByIdAsync(subLocationId, SubLocationConstants.IncludeLocation)
            ?? throw new KeyNotFoundException(string.Format(SubLocationConstants.SubLocationNotFoundMessage, subLocationId));
        
        if (subLocation.Location?.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(SubLocationConstants.UnauthorizedAccessMessage, userId, subLocationId));
        }
        
        return subLocation;
    }
    
    public async Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId)
    {
        var location = await _repository.GetByIdAsync<Location>(locationId)
            ?? throw new KeyNotFoundException(string.Format(SubLocationConstants.LocationNotFoundMessage, locationId));
        
        if (location.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(SubLocationConstants.UnauthorizedLocationAccessMessage, userId, locationId));
        }
        
        return location;
    }
}