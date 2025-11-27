﻿using AuditSystem.Auth.Dtos.Password;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Authentication;

/// <summary>
/// Controller for secure password reset operations using ASP.NET Identity tokens.
/// </summary>
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
    /// <summary>
    /// Step 1: User requests password reset by providing their email.
    /// Generates a secure token and sends it via email.
    /// </summary>
    [HttpPost("forgot-password")]
    [PreventBruteForce]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(ApiResponse.BadRequest("Email is required."));
        }

        try
        {
            // Find user by email
            var user = await _accountService.FindByEmailAsync(request.Email);

            // SECURITY: Always return success even if user doesn't exist
            // This prevents email enumeration attacks
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
                // Return success to prevent email enumeration
                return Ok(ApiResponse.Ok("If the email exists in our system, a password reset link has been sent."));
            }

            // Generate secure token (cryptographically secure, time-limited, one-time use)
            var token = await _passwordTokenService.GeneratePasswordResetTokenAsync(user);

            // Build reset URL with encoded token
            // TODO: Replace with your actual frontend URL
            var resetUrl = $"{Request.Scheme}://{Request.Host}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";

            // Send password reset email with the secure token link
            await _emailService.SendPasswordResetEmailAsync(user.Email!, resetUrl);

            _logger.LogInformation("Password reset token generated and email sent for user: {Email}", request.Email);

            // Return success (same response whether user exists or not)
            return Ok(ApiResponse.Ok("If the email exists in our system, a password reset link has been sent."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during password reset request for email: {Email}", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse.InternalServerError("An unexpected error occurred. Please try again later."));
        }
    }

    /// <summary>
    /// Step 2: User submits new password with the token from their email.
    /// Validates the token and resets the password.
    /// </summary>
    [HttpPost("reset-password")]
    [PreventBruteForce]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(ApiResponse.BadRequest("Email is required."));

        if (string.IsNullOrWhiteSpace(request.Token))
            return BadRequest(ApiResponse.BadRequest("Reset token is required."));

        if (string.IsNullOrWhiteSpace(request.NewPassword))
            return BadRequest(ApiResponse.BadRequest("New password is required."));

        if (request.NewPassword != request.ConfirmNewPassword)
            return BadRequest(ApiResponse.BadRequest("Passwords do not match."));

        try
        {
            // Find user by email
            var user = await _accountService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
                return BadRequest(ApiResponse.BadRequest("Invalid reset request."));
            }

            // Reset password using the secure token
            // Identity automatically validates:
            // - Token hasn't expired (default: 1 day, configurable)
            // - Token hasn't been used before (one-time use)
            // - Token hasn't been tampered with (cryptographic validation)
            var result = await _passwordResetService.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new ApiErrorResponse(e.Description));
                _logger.LogWarning("Password reset failed for user {Email}: {Errors}", 
                    request.Email, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(ApiResponse.BadRequest(errors));
            }

            _logger.LogInformation("Password successfully reset for user: {Email}", request.Email);

            // Send confirmation email
            await _emailService.SendPasswordChangedConfirmationAsync(user.Email!);

            // Invalidate all existing refresh tokens/sessions for security
            await _accountService.RevokeAllRefreshTokensAsync(user);

            return Ok(ApiResponse.Ok("Password has been reset successfully. You can now log in with your new password."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during password reset for email: {Email}", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse.InternalServerError("An unexpected error occurred. Please try again later."));
        }
    }
}