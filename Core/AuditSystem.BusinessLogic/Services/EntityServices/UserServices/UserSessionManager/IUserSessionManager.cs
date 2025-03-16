using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserSessionManager;

public interface IUserSessionManager
{
    TimeSpan GetCurrentSessionDuration(User user);
    void StartSession(User user);
    void EndSession(User user);
    TimeSpan GetTotalSessionTime(User user);
}