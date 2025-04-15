using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserManagementServices;

public interface IUserDesignationService
{
    public Task<Guid> CreateUserDesignationAsync(UserDesignationModel userDesignationModel);
    public Task UpdateUserDesignationAsync(UserDesignationModel userDesignationModel);
    public Task DeleteUserDesignationAsync(Guid userDesignationId);
}