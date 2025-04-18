namespace AuditSystem.Contract.Interfaces.ModelServices.UserServices;

public interface IUserSessionManager
{
    TimeSpan GetCurrentSessionDuration(User user);
    void StartSession(User user);
    void EndSession(User user);
    TimeSpan GetTotalSessionTime(User user);
}