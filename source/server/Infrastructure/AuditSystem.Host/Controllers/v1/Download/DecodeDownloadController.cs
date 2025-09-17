using Ardalis.Result;
using AuditSystem.Application.Features.DownloadFile.Get;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Download;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DecodeDownloadController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public DecodeDownloadController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> DecodeAndDownload([FromQuery] string signedUrl)
    {
        var result = await _mediator.Send(new GetFileByURLQuery(signedUrl));

        if (!result.IsSuccess)
        {
            return result.Status switch
            {
                ResultStatus.NotFound => NotFound(result.Errors),
                ResultStatus.Unauthorized => Unauthorized(result.Errors),
                _ => BadRequest(result.Errors)
            };
        }

        return File(
            result.Value.FileStream,
            result.Value.ContentType,
            result.Value.FileName);
    }
}