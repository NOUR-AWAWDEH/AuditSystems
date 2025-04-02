using AuditSystem.Auth.Common.ApiResponses;
using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

    public AuthenticationController(
        ICustomAuthenticationService authService,
        IAccountService accountService,
        IEmailService emailService,
        ILogger<AuthenticationController> logger,
        IPasswordResetService passwordResetService,
        IPasswordTokenService passwordTokenService,
        IRegistrationService registrationService)
    {
        _authService = authService;
        _accountService = accountService;
        _emailService = emailService;
        _logger = logger;
        _passwordResetService = passwordResetService;
        _passwordTokenService = passwordTokenService;
        _registrationService = registrationService;
    }

    [HttpPost("login")]
    [EnableRateLimiting("loginPolicy")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> LoginAsync([FromBody] LoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ApiResponse<LoginResponseDto>(false, "Invalid request model"));

        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
            return Unauthorized(new ApiResponse<LoginResponseDto>(false, $"Failed login attempt for user name {request.Username}"));
        }

        if (!response.IsEmailVerified)
        {
            _logger.LogWarning("Login attempt for unverified email {Email}", request.Username);
            return Unauthorized(new ApiResponse<LoginResponseDto>(false, "Email not confirmed. Please verify your email."));
        }

        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        return Ok(new ApiResponse<LoginResponseDto>(true, "Login successful", response));
    }


    [HttpPost("authentication-status")]
    public async Task<ActionResult<ApiResponse<AuthStatusResponseDto>>> AuthStatusAsync([FromBody] AuthStatusDto request)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(request.Token))
        {
            return BadRequest(new ApiResponse<AuthStatusResponseDto>(false, "Invalid request model or missing token."));
        }

        var response = await _authService.GetAuthStatusFromToken(request);
        if (response == null || !response.IsAuthenticated)
        {
            _logger.LogWarning("Failed authentication attempt.");
            return Unauthorized(new ApiResponse<AuthStatusResponseDto>(false, "Invalid token or user not found."));
        }

        _logger.LogInformation("User {Username} authenticated successfully", response.Username);
        return Ok(new ApiResponse<AuthStatusResponseDto>(true, "Authentication successful", response));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> LogOut()
    {
        await _authService.SignOutAsync();
        return Ok(new ApiResponse(true, "Signed out successfully"));
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse>> ResetPassword([FromBody] ResetPasswordDto request)
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

    [HttpPost("verify-reset-code")]
    public async Task<ActionResult<ApiResponse<string>>> VerifyResetCode([FromBody] VerifyResetCodeDto request)
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

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse>> ForgotPassword([FromBody] ForgotPasswordRequest request)
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

        // Using the new strongly-typed email request
        await _emailService.SendEmailAsync(
            new PasswordResetEmailRequest(
                email: request.Email,
                userName: user.UserName,
                resetCode: resetCode,
                supportEmail: "support@yourapp.com"
            )
        );

        return Ok(new ApiResponse(true, "Password reset code sent successfully."));
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
                return BadRequest(new ApiResponse(false, "Invalid model state", ModelState));

            // Pass `Request.Scheme` and `_urlHelper` to the registration service
            await _registrationService.RegisterAsync(model, Request.Scheme, Url);

            return Ok(new ApiResponse(true, "User registered successfully. Please check your email for verification."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for {Email}", model.Email);
            return StatusCode(500, new ApiResponse(false, "An unexpected error occurred"));
        }
    }

    [HttpGet("confirm-email")]
    public async Task<ActionResult<ApiResponse>> ConfirmEmail(string uid, string token)
    {
        if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
        {
            token = token.Replace(' ', '+');
            var result = await _accountService.ConfirmEmailAsync(uid, token);
            if (result.Succeeded)
                return Ok(new ApiResponse(true, "Email confirmed successfully."));
            else
                return BadRequest(new ApiResponse(false, "Email confirmation failed."));
        }
        return BadRequest(new ApiResponse(false, "Invalid user id or token."));
    }

}