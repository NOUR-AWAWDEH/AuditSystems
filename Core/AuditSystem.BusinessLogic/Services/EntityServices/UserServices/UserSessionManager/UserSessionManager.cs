using AuditSystem.Domain.Entities.Users;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserSessionManager;

public class UserSessionManager : IUserSessionManager
{
    public TimeSpan GetCurrentSessionDuration(User user)
    {
        return user.CurrentSessionStart.HasValue 
            ? DateTime.UtcNow - user.CurrentSessionStart.Value 
            : TimeSpan.Zero;
    }

    public void StartSession(User user)
    {
        user.CurrentSessionStart = DateTime.UtcNow;
        user.LastLogin = DateTime.UtcNow;
        user.IsActive = true;
    }

    public void EndSession(User user)
    {
        if (user.CurrentSessionStart.HasValue)
        {
            user.TotalSessionTime += GetCurrentSessionDuration(user);
            user.CurrentSessionStart = null;
            user.IsActive = false;
        }
    }

    public TimeSpan GetTotalSessionTime(User user)
    {
        var totalTime = user.TotalSessionTime;
        if (user.IsActive && user.CurrentSessionStart.HasValue)
        {
            totalTime += GetCurrentSessionDuration(user);
        }
        return totalTime;
    }
}