using AuditSystem.Auth.Common.ApiResponses;
using AuditSystem.Auth.Dtos;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    IRegistrationService _registrationService;
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(
        IRegistrationService registrationService,
        ILogger<RegistrationController> logger)
    {
        _registrationService = registrationService;
        _logger = logger;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterDto model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(false, "Invalid model state", ModelState));

            var result = await _registrationService.RegisterAsync(model);

            if (result == null)
                return StatusCode(500, new ApiResponse(false, "An unexpected error occurred during registration."));

            return result.Succeeded
                ? Ok(new ApiResponse(true, "User registered successfully"))
                : BadRequest(new ApiResponse(false, "User registration failed", result.Errors.Select(e => e.Description)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration for {Email}", model.Email);
            return StatusCode(500, new ApiResponse(false, "An unexpected error occurred"));
        }
    }
}
