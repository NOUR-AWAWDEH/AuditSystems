using AuditSystem.Auth.Common.ApiResponses;
using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Services.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace AuditSystem.Host.Controllers.v1.Authentication;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthenticationController : ControllerBase
{
    private readonly ICustomAuthenticationService _authService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
       ICustomAuthenticationService authService,
        ILogger<AuthenticationController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("loginPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> LoginAsync([FromBody] LoginDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<LoginResponseDto>(false, "Invalid request model"));
            }

            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                _logger.LogWarning("Failed login attempt for user {Username}", request.Username);
                return Unauthorized(new ApiResponse<LoginResponseDto>(false, "Invalid credentials"));
            }

            _logger.LogInformation("User {Username} logged in successfully", request.Username);
            return Ok(new ApiResponse<LoginResponseDto>(true, "Login successful", response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Username}", request.Username);
            return StatusCode(500, new ApiResponse<LoginResponseDto>(false, "An unexpected error occurred"));
        }
    }

    [HttpPost("logOut")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> LogOut()
    {
        try
        {
            await _authService.SignOutAsync();

            return Ok(new ApiResponse(true, "Signed out successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign out");
            return StatusCode(500, new ApiResponse(false, "An unexpected error occurred"));
        }
    }
}