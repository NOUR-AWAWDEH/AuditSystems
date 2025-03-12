namespace AuditSystem.Auth.Dtos
{
    public class SignOutRequestDto
    {
        public required Guid userId {get;set;} = string.Empty;
    }
}
