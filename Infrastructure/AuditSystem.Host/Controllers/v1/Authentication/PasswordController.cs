using AuditSystem.Auth.Dtos.Password;
using AuditSystem.Auth.Dtos.Register;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Host.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Authentication;

[Route("api/[controller]")]
[ApiController]
public sealed class PasswordController(
        IAccountService _accountService,
        IEmailService _emailService,
        IPasswordResetService _passwordResetService,
        IPasswordTokenService _passwordTokenService,
        ILogger<PasswordController> _logger) :
        ControllerBase
{
    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(ApiResponse.BadRequest("Email is required"));

        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
            return NotFound(ApiResponse.NotFound("User"));
        }

        var resetCode = _passwordResetService.GenerateResetCode();
        await _passwordResetService.SaveResetCodeAsync(request.Email, resetCode);

        await _emailService.SendEmailAsync(new PasswordResetEmailRequest(
            email: request.Email,
            userName: user.UserName ?? string.Empty,
            resetCode: resetCode,
            supportEmail: "support@yourapp.com"
        ));

        return Ok(ApiResponse.Ok("Password reset code sent successfully"));
    }

    [HttpPost("verify-reset-code")]
    public async Task<ActionResult<ApiResponse<string>>> VerifyResetCode([FromBody] VerifyResetCodeDto request)
    {
        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound(ApiResponse<string>.NotFound("User"));

        var isValid = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.Code);
        if (!isValid)
            return BadRequest(ApiResponse<string>.BadRequest("Invalid or expired reset code"));

        var token = await _passwordTokenService.GenerateAndStorePasswordResetTokenAsync(user);
        return Ok(ApiResponse<string>.Ok(token, "Reset code verified successfully"));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return BadRequest(ApiResponse.BadRequest("Passwords do not match"));

        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound(ApiResponse.NotFound("User"));

        if (!_passwordTokenService.ValidateResetTokenAsync(user, request.Token))
            return BadRequest(ApiResponse.BadRequest("Invalid or expired reset token"));

        var result = await _passwordResetService.ResetPasswordAsync(user, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new ApiErrorResponse(e.Description));
            return BadRequest(ApiResponse.BadRequest(errors));
        }

        await _passwordResetService.ClearResetCodeAsync(user);
        return Ok(ApiResponse.Ok("Password reset successfully"));
    }
}
