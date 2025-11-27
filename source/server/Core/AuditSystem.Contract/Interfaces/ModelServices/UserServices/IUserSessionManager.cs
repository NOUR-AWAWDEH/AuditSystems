using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices;

public interface IUserSessionManager
{
    Task StartSessionAsync(AppUser user);
    Task EndSessionAsync(AppUser user);
    Task<TimeSpan> GetCurrentSessionDurationAsync(AppUser user);
    Task<TimeSpan> GetTotalSessionTimeAsync(AppUser user);
}
