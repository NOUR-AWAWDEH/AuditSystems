using AuditSystem.Contract.Interfaces.ModelServices.UserServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Services.EntityServices.UserServices.UserSessionManager;
public class UserSessionManager : IUserSessionManager
{
    private readonly IRepository<Guid, UserSessionInfo> _sessionRepository;

    public UserSessionManager(IRepository<Guid, UserSessionInfo> sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task StartSessionAsync(AppUser user)
    {
        var session = new UserSessionInfo
        {
            Id = Guid.NewGuid(),
            AppUserId = user.Id,
            CurrentSessionStart = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow,
            IsActive = true
        };

        await _sessionRepository.CreateAsync(session);
    }

    public async Task EndSessionAsync(AppUser user)
    {
        var activeSession = await _sessionRepository.Query()
            .FirstOrDefaultAsync(s => s.AppUserId == user.Id && s.IsActive);

        if (activeSession is null) return;

        activeSession.TotalSessionTime += DateTime.UtcNow - activeSession.CurrentSessionStart!.Value;
        activeSession.CurrentSessionStart = null;
        activeSession.IsActive = false;

        await _sessionRepository.UpdateAsync(activeSession);
    }

    public async Task<TimeSpan> GetCurrentSessionDurationAsync(AppUser user)
    {
        var activeSession = await _sessionRepository.Query()
            .FirstOrDefaultAsync(s => s.AppUserId == user.Id && s.IsActive);

        if (activeSession == null || !activeSession.CurrentSessionStart.HasValue)
            return TimeSpan.Zero;

        return DateTime.UtcNow - activeSession.CurrentSessionStart.Value;
    }

    public async Task<TimeSpan> GetTotalSessionTimeAsync(AppUser user)
    {
        var sessions = await _sessionRepository.Query()
            .Where(s => s.AppUserId == user.Id)
            .ToListAsync();

        TimeSpan total = sessions.Aggregate(TimeSpan.Zero, (sum, s) => sum + s.TotalSessionTime);

        var activeSession = sessions.FirstOrDefault(s => s.IsActive && s.CurrentSessionStart.HasValue);
        if (activeSession != null)
            total += DateTime.UtcNow - activeSession.CurrentSessionStart!.Value;

        return total;
    }
}
