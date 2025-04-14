using AuditSystem.Auth.Dtos.Register;
using AuditSystem.Auth.Services.Registration;
using AuditSystem.Host.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace AuditSystem.Host.Controllers.v1.Authentication;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(
        IRegistrationService registrationService,
        UserManager<User> userManager,
        ILogger<RegistrationController> logger)
    {
        _registrationService = registrationService;
        _userManager = userManager;
        _logger = logger;
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

    //verification-email for old account
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
