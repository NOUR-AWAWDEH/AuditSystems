namespace AuditSystem.Contract.Models.Common;

public class BaseModel<TId>
    where TId : IComparable<TId>
{
    public required TId Id { get; set; }

}