using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.BusinessLogic.Services.UserSession
{
    public interface IUserSessionManager
    {
        TimeSpan GetCurrentSessionDuration(User user);
        void StartSession(User user);
        void EndSession(User user);
        TimeSpan GetTotalSessionTime(User user);
    }
}