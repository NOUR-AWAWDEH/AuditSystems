namespace AuditSystem.Contract.Models.RateLimiting;

public class BruteForceProtectionEntry
{
    public int Requests { get; set; }
    public int BlockCount { get; set; }
    public DateTime BlockedUntil { get; set; } = DateTime.MinValue;
}