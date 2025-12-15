using AuditSystem.Auth.Dtos.AuthStatus;
using AuditSystem.Auth.Dtos.Login;
using AuditSystem.Auth.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.Contract.Common;
using AuditSystem.Auth.Dtos.RefreshToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuditSystem.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.Host.Controllers.v1.Authentication;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController(
        ICustomAuthenticationService _authService) :
        ControllerBase
{

    [HttpPost("login")]
    [PreventBruteForce]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ApiResponse>> LogOut()
    {
        var result = await _authService.SignOutAsync();

        if (!result)
            return BadRequest(ApiResponse.BadRequest("Logout failed"));

        return Ok(ApiResponse.Ok("Logged out successfully from all sessions"));
    }

    [HttpPost("logout-all")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthResultDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(ApiResponse<AuthResultDto>.BadRequest("Refresh token is required"));

        var response = await _authService.RefreshTokenAsync(request.RefreshToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("auth-status")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAuthStatus([FromBody] AuthStatusRequestDto request)
    {
        var result = await _authService.GetAuthStatusFromToken(request);
        if (result == null || !result.IsAuthenticated)
            return Unauthorized(result);

        return Ok(result);
    }
}