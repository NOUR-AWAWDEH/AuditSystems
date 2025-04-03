using AuditSystem.Auth.Common.ApiResponses;
using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Text;

namespace AuditSystem.Host.Controllers.v1.Authentication;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomAuthenticationService _authService;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;
    private readonly IPasswordResetService _passwordResetService;
    private readonly IPasswordTokenService _passwordTokenService;
    private readonly IRegistrationService _registrationService;
    private readonly UserManager<User> _userManager;

    public AuthenticationController(
        ICustomAuthenticationService authService,
        IAccountService accountService,
        IEmailService emailService,
        ILogger<AuthenticationController> logger,
        IPasswordResetService passwordResetService,
        IPasswordTokenService passwordTokenService,
        IRegistrationService registrationService,
        UserManager<User> userManager)
    {
        _authService = authService;
        _accountService = accountService;
        _emailService = emailService;
        _logger = logger;
        _passwordResetService = passwordResetService;
        _passwordTokenService = passwordTokenService;
        _registrationService = registrationService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    [EnableRateLimiting("loginPolicy")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> LoginAsync([FromBody] LoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<LoginResponseDto>.ErrorResponse("Invalid request model"));

        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse(
                $"Failed login attempt for user name {request.Username}",
                "LOGIN_FAILED",
                "Invalid credentials"));
        }

        if (!response.IsEmailVerified)
        {
            _logger.LogWarning("Login attempt for unverified email {Email}", request.Username);
            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse(
                "Email not confirmed. Please verify your email.",
                "EMAIL_NOT_VERIFIED",
                "Account requires email verification"));
        }

        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response));
    }

    [HttpPost("authentication-status")]
    public async Task<ActionResult<ApiResponse<AuthStatusResponseDto>>> AuthStatusAsync([FromBody] AuthStatusDto request)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(request.Token))
        {
            return BadRequest(ApiResponse<AuthStatusResponseDto>.ErrorResponse(
                "Invalid request model or missing token",
                "INVALID_REQUEST",
                "Token is required"));
        }

        var response = await _authService.GetAuthStatusFromToken(request);
        if (response == null || !response.IsAuthenticated)
        {
            _logger.LogWarning("Failed authentication attempt.");
            return Unauthorized(ApiResponse<AuthStatusResponseDto>.ErrorResponse(
                "Invalid token or user not found",
                "AUTH_FAILED",
                "Authentication token is invalid or expired"));
        }

        _logger.LogInformation("User {Username} authenticated successfully", response.Username);
        return Ok(ApiResponse<AuthStatusResponseDto>.SuccessResponse(response));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> LogOut()
    {
        await _authService.SignOutAsync();
        return Ok(ApiResponse.SuccessResponse("Signed out successfully"));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
    {
        if (request.NewPassword != request.ConfirmNewPassword)
            return BadRequest(ApiResponse.ErrorResponse(
                "Passwords do not match",
                "PASSWORD_MISMATCH",
                "New password and confirmation must match"));

        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest(ApiResponse.NotFound("User"));

        if (!_passwordTokenService.ValidateResetTokenAsync(user, request.Token))
            return BadRequest(ApiResponse.ErrorResponse(
                "Invalid or expired reset token",
                "INVALID_TOKEN",
                "Reset token validation failed"));

        var result = await _passwordResetService.ResetPasswordAsync(user, request.NewPassword);
        if (!result.Succeeded)
            return BadRequest(ApiResponse.ErrorResponse(
                "Password reset failed",
                result.Errors.Select(e => new ErrorDetail("RESET_FAILED", e.Description))));

        await _passwordResetService.ClearResetCodeAsync(user);
        return Ok(ApiResponse.SuccessResponse("Password reset successfully"));
    }

    [HttpPost("verify-reset-code")]
    public async Task<ActionResult<ApiResponse<string>>> VerifyResetCode([FromBody] VerifyResetCodeDto request)
    {
        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest(ApiResponse<string>.NotFound("User"));

        var isValid = await _passwordResetService.VerifyResetCodeAsync(request.Email, request.Code);
        if (!isValid)
            return BadRequest(ApiResponse<string>.ErrorResponse(
                "Invalid or expired reset code",
                "INVALID_CODE",
                "Reset code validation failed"));

        var token = await _passwordTokenService.GenerateAndStorePasswordResetTokenAsync(user);
        return Ok(ApiResponse<string>.SuccessResponse(token, "Reset code verified successfully"));
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return BadRequest(ApiResponse.ErrorResponse(
                "Email is required",
                "MISSING_EMAIL",
                "Email field cannot be empty"));

        var user = await _accountService.FindByEmailAsync(request.Email);
        if (user == null)
        {
            _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
            return NotFound(ApiResponse.NotFound($"User"));
        }

        var resetCode = _passwordResetService.GenerateResetCode();
        await _passwordResetService.SaveResetCodeAsync(request.Email, resetCode);

        await _emailService.SendEmailAsync(
            new PasswordResetEmailRequest(
                email: request.Email,
                userName: user.UserName,
                resetCode: resetCode,
                supportEmail: "support@yourapp.com"
            )
        );

        return Ok(ApiResponse.SuccessResponse("Password reset code sent successfully"));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(e => new ValidationError(
                        errorCode: "VALIDATION_ERROR",
                        errorMessage: e.ErrorMessage,
                        propertyName: ModelState.Keys.FirstOrDefault(k => ModelState[k].Errors.Contains(e))
                    ));
                return BadRequest(ApiResponse.ValidationError(errors));
            }

            await _registrationService.RegisterAsync(model, Request.Scheme, Url);
            return Ok(ApiResponse.SuccessResponse("User registered successfully. Please check your email for verification"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for {Email}", model.Email);
            return StatusCode(500, ApiResponse.ErrorResponse(
                "An unexpected error occurred",
                "INTERNAL_ERROR",
                ex.Message));
        }
    }


    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string uid, [FromQuery] string token)
    {
        // Validate parameters
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Missing uid or token." });

        // Find user
        var user = await _userManager.FindByIdAsync(uid);
        if (user == null)
            return BadRequest("Invalid user");

        try
        {
            // Decode token
            var base64DecodedToken = WebUtility.UrlDecode(token);
            var decodedBytes = Convert.FromBase64String(base64DecodedToken);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            // Confirm email
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded)
                return BadRequest("Invalid token");

            // Explicitly set EmailConfirmed to true
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return Ok("Email confirmed successfully");
        }
        catch (FormatException)
        {
            return BadRequest("Invalid token format");
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            return StatusCode(500, "An error occurred while confirming email");
        }
    }
}