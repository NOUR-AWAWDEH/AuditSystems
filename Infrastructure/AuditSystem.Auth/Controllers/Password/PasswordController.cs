using AuditSystem.Auth.Common.ApiResponses;
using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Auth.Controllers.Account;
[Route("api/[controller]")]
[ApiController]
public class PasswordController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;
    private readonly ILogger<PasswordController> _logger;
    private readonly IPasswordResetService _passwordResetService;
    private readonly IPasswordTokenService _passwordTokenService;

    public PasswordController(
        IAccountService accountService,
        IEmailService emailService,
        ILogger<PasswordController> logger,
        IPasswordResetService passwordResetService,
        IPasswordTokenService passwordTokenService)
    {
        _accountService = accountService;
        _emailService = emailService;
        _logger = logger;
        _passwordResetService = passwordResetService;
        _passwordTokenService = passwordTokenService;
    }


    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest(new ApiResponse(false, "Email is required."));

            var user = await _accountService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
                return NotFound(new ApiResponse(false, "User not found."));
            }

            var resetCode = _passwordResetService.GenerateResetCode();
            await _passwordResetService.SaveResetCodeAsync(request.Email, resetCode);

            await _emailService.SendEmailAsync(
                request.Email,
                "Password Reset Code",
                $"Your password reset code is: <b>{resetCode}</b>");

            return Ok(new ApiResponse(true, "Password reset code sent successfully."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for {Email}", request.Email);
            return StatusCode(500, new ApiResponse(false, "An unexpected error occurred"));
        }
    }

    [HttpPost("verify-reset-code")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<string>>> VerifyResetCode([FromBody] VerifyResetCodeDto request)
    {
        try
        {
            var user = await _accountService.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ApiResponse<string>(false, "User not found."));

            var isValid = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.Code);
            if (!isValid)
                return BadRequest(new ApiResponse<string>(false, "Invalid or expired reset code."));

            var token = await _passwordTokenService.GenerateAndStorePasswordResetTokenAsync(user);
            return Ok(new ApiResponse<string>(true, "Reset code verified successfully.", token));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during reset code verification for {Email}", request.Email);
            return StatusCode(500, new ApiResponse<string>(false, "An unexpected error occurred"));
        }
    }

    [HttpPost("reset")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
    {
        try
        {
            if (request.NewPassword != request.ConfirmNewPassword)
                return BadRequest(new ApiResponse(false, "Passwords do not match."));

            var user = await _accountService.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new ApiResponse(false, "User not found."));

            if (!_passwordTokenService.ValidateResetTokenAsync(user, request.Token))
                return BadRequest(new ApiResponse(false, "Invalid or expired reset token."));

            var result = await _passwordResetService.ResetPasswordAsync(user, request.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(false, "Password reset failed", result.Errors.Select(e => e.Description)));

            await _passwordResetService.ClearResetCodeAsync(user);
            return Ok(new ApiResponse(true, "Password reset successfully."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset for {Email}", request.Email);
            return StatusCode(500, new ApiResponse(false, "An unexpected error occurred"));
        }
    }
}

