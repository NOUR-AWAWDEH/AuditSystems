namespace AuditSystem.Contract.Interfaces;

public interface IUserAwareRequest
{
    Guid UserId { get; set; }
}