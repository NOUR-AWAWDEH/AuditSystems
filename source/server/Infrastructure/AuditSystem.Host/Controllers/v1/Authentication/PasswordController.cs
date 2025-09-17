﻿﻿﻿﻿using AuditSystem.Auth.Dtos.Password;
using AuditSystem.Auth.Dtos.Register;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
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
    [PreventBruteForce]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        // 1. Input Validation: Email is required
        if (string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(ApiResponse.BadRequest("Email is required."));
        }

        try
        {
            // 2. Find User: Check if the user exists
            var user = await _accountService.FindByEmailAsync(request.Email);

            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
                return NotFound(ApiResponse.NotFound(new List<ApiErrorResponse>
                {
                    new ApiErrorResponse($"The user was not found.")
                }));
            }

            // 3. Generate and Save Reset Code
            var resetCode = _passwordResetService.GenerateResetCode();
            await _passwordResetService.SaveResetCodeAsync(request.Email, resetCode);

            // 4. Send Password Reset Email
            await _emailService.SendEmailAsync(new PasswordResetEmailRequest(
                email: request.Email,
                userName: user.UserName ?? string.Empty,
                resetCode: resetCode,
                supportEmail: "support@AuditSystem.com"
            ));

            // 5. Success Response
            return Ok(ApiResponse.Ok("Password reset code sent successfully. Please check your email."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during password reset for email: {Email}", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.InternalServerError("An unexpected error occurred. Please try again later."));
        }
    }

    [HttpPost("verify-reset-code")]
    [PreventBruteForce]
    public async Task<ActionResult<ApiResponse<VerifyResetCodeResponseDto>>> VerifyResetCode([FromBody] VerifyResetCodeDto request)
    {
        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound(ApiResponse<VerifyResetCodeResponseDto>.NotFound("User not found"));

        var isValid = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.Code);
        if (!isValid)
            return BadRequest(ApiResponse<VerifyResetCodeResponseDto>.BadRequest("Invalid or expired reset code"));

        var token = await _passwordTokenService.GenerateAndStorePasswordResetTokenAsync(user);
        return Ok(ApiResponse<VerifyResetCodeResponseDto>.Ok(new VerifyResetCodeResponseDto
        {
            ResetToken = token,
            Success = true,
            SuccessMessage = "Reset code verified successfully",
            StatusCode = 200
        }));
    }

    [HttpPost("reset-password")]
    [PreventBruteForce]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return BadRequest(ApiResponse.BadRequest("Passwords do not match"));

        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
            return NotFound(ApiResponse.NotFound(new List<ApiErrorResponse> { new ApiErrorResponse("User") }));

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
