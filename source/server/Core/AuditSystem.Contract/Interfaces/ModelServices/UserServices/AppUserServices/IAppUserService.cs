using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Contract.Dtos.Register;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;

public interface IAppUserService
{
    Task CreateUserAsync(RegisterDto request, Guid adminUserId, string scheme);
    Task UpdateUserAsync(UserModel user, Guid adminUserId);
    Task DeleteUserAsync(Guid userId, Guid adminUserId);
    Task<UserModel> GetUserByIdAsync(Guid id, Guid adminUserId);
    Task<IReadOnlyList<UserModel>> GetUsersByAdminIdAsync(Guid adminUserId);

    // get all the users created by spesific admin with pagination 
    Task<PaginationResult<UserModel>> SearchByAdminIdAsync(Guid adminUserId, string? searchTerm, Page page);

    //ActionOwner
    Task<List<UserModel>> GetActionOwnersByUserIdAsync(Guid adminUserId);
}