namespace AuditSystem.Auth.Dtos.Password;

public class VerifyResetCodeResponseDto
{
    public string ResetToken { get; set; } = string.Empty;
    public bool Success { get; set; } 
    public string SuccessMessage { get; set; } = string.Empty;
    public int StatusCode { get; set; } 
}