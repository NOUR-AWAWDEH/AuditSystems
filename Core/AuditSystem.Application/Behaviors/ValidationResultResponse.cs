namespace AuditSystem.Application.Behaviors;

public class ValidationResultResponse<TResponse>
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new();
    public TResponse Response { get; set; }
}