using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.Auth.Dtos.Logout;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Host.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace AuditSystem.Host.Controllers.v1.Authentication;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController(
        ICustomAuthenticationService _authService,
        ILogger<AuthController> _logger) :
        ControllerBase
{

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
    [Authorize]
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
}