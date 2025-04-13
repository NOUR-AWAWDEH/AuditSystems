using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.Auth.Dtos.Logout;
using AuditSystem.Auth.Dtos.Password;
using AuditSystem.Auth.Dtos.Register;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using AuditSystem.Host.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net;
using System.Security.Claims;
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
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> LoginAsync([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<LoginResponseDto>.BadRequest("Invalid request model"));

        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
            return Unauthorized(ApiResponse<LoginResponseDto>.Unauthorized("Invalid credentials"));
        }

        if (!response.IsEmailVerified)
        {
            _logger.LogWarning("Login attempt for unverified email {Email}", request.Username);
            return Unauthorized(ApiResponse<LoginResponseDto>.Unauthorized("Email not confirmed. Please verify your email."));
        }

        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        return Ok(ApiResponse<LoginResponseDto>.Ok(response));
    }

    [HttpPost("authentication-status")]
    public async Task<ActionResult<ApiResponse<AuthStatusResponseDto>>> AuthStatusAsync([FromBody] AuthStatusRequestDto request)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(request.Token))
            return BadRequest(ApiResponse<AuthStatusResponseDto>.BadRequest("Token is required"));

        var response = await _authService.GetAuthStatusFromToken(request);
        if (response == null || !response.IsAuthenticated)
        {
            _logger.LogWarning("Failed authentication attempt.");
            return Unauthorized(ApiResponse<AuthStatusResponseDto>.Unauthorized("Authentication token is invalid or expired"));
        }

        _logger.LogInformation("User {Username} authenticated successfully", response.Username);
        return Ok(ApiResponse<AuthStatusResponseDto>.Ok(response));
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> LogOut([FromBody] LogoutRequest request)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            return BadRequest(ApiResponse.BadRequest("Refresh token is required"));

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.Unauthorized("User not found"));

        var success = await _authService.RevokeRefreshTokenAsync(userId, request.RefreshToken);
        if (!success)
            return BadRequest(ApiResponse.BadRequest("Failed to revoke refresh token"));

        return Ok(ApiResponse.Ok("Logged out successfully"));
    }

    [HttpPost("logout-all")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> LogOutAll()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(ApiResponse.Unauthorized("User not found"));

        var success = await _authService.RevokeAllRefreshTokensAsync(userId);
        if (!success)
            return BadRequest(ApiResponse.BadRequest("No active sessions found to revoke"));

        return Ok(ApiResponse.Ok("Logged out from all sessions successfully"));
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

        await _emailService.SendEmailAsync(
            new PasswordResetEmailRequest(
                email: request.Email,
                userName: user.UserName,
                resetCode: resetCode,
                supportEmail: "support@yourapp.com"
            )
        );

        return Ok(ApiResponse.Ok("Password reset code sent successfully"));
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterDto model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(e => new ApiErrorResponse(e.ErrorMessage));
                return BadRequest(ApiResponse.BadRequest(errors));
            }

            await _registrationService.RegisterAsync(model, Request.Scheme, Url);
            return Ok(ApiResponse.Ok("User registered successfully. Please check your email for verification"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for {Email}", model.Email);
            return StatusCode(500, ApiResponse.InternalServerError("An unexpected error occurred"));
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string uid, [FromQuery] string token)
    {
        if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
            return BadRequest(new { message = "Missing uid or token." });

        var user = await _userManager.FindByIdAsync(uid);
        if (user == null)
            return BadRequest("Invalid user");

        try
        {
            var base64DecodedToken = WebUtility.UrlDecode(token);
            var decodedBytes = Convert.FromBase64String(base64DecodedToken);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded)
                return BadRequest("Invalid token");

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            return Ok("Email confirmed successfully");
        }
        catch (FormatException)
        {
            return BadRequest("Invalid token format");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while confirming email");
        }
    }

    [HttpPost("send-verification-email")]
    public async Task<ActionResult<ApiResponse>> SendVerificationEmail([FromBody] VerificationEmailDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors)
                    .Select(e => new ApiErrorResponse(e.ErrorMessage));
                return BadRequest(ApiResponse.BadRequest(errors));
            }

            await _registrationService.VerificationEmailAsync(request, Request.Scheme, Url);
            return Ok(ApiResponse.Ok("Verification email sent successfully. Please check your inbox."));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Verification email failed for non-existing user {Email}", request.Email);
            return BadRequest(ApiResponse.BadRequest(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending verification email to {Email}", request.Email);
            return StatusCode(500, ApiResponse.InternalServerError("An unexpected error occurred"));
        }
    }
}
