using Ardalis.Result;
using AuditSystem.Application.Features.DownloadFile.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Controllers.v1.Download;

[ApiController]
[Route("[controller]")]
public class DecodeDownloadController(IMediator mediator) : ApiControllerBase(mediator)
{

    [HttpGet]
    public async Task<IActionResult> DecodeAndDownload([FromQuery] string signedUrl)
    {
        var result = await mediator.Send(new GetFileByURLQuery(signedUrl));

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



//using AuditSystem.Contract.Interfaces.FileStorage;
//using Microsoft.AspNetCore.Mvc;
//using System.Web;

//namespace AuditSystem.Host.Controllers.v1.Download
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class DecodeDownloadController(IFilePathBuilder filePathBuilder,
//    ISignedUrlService signedUrlService,
//    ILogger<DecodeDownloadController> logger)
//    : ControllerBase
//    {
//        private readonly ILogger<DecodeDownloadController> _logger = logger;
//        private readonly IFilePathBuilder _filePathBuilder = filePathBuilder;
//        private readonly ISignedUrlService _signedUrlService = signedUrlService;

//        [HttpGet]
//        public IActionResult DecodeAndDownload([FromQuery] string signedUrl)
//        {
//            _logger.LogInformation("Attempting to decode and download file with signed URL: {SignedUrl}", signedUrl);

//            if (!Uri.TryCreate(signedUrl, UriKind.Absolute, out var uri))
//            {
//                _logger.LogWarning("Invalid signed URL provided: {SignedUrl}", signedUrl);
//                return BadRequest("Invalid signed URL.");
//            }

//            var query = HttpUtility.ParseQueryString(uri.Query);
//            var file = query["file"];
//            var expires = query["expires"];
//            var signature = query["signature"];

//            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(expires) || string.IsNullOrEmpty(signature))
//            {
//                _logger.LogWarning("Missing query parameters in signed URL: file={File}, expires={Expires}, signature={Signature}", file, expires, signature);
//                return BadRequest("Missing query parameters.");
//            }

//            var decodedFile = Uri.UnescapeDataString(file);
//            var decodedSignature = Uri.UnescapeDataString(signature);

//            if (!long.TryParse(expires, out var expiresUnix))
//            {
//                _logger.LogWarning("Invalid expires timestamp in signed URL: {Expires}", expires);
//                return BadRequest("Invalid expires timestamp.");
//            }

//            // Use SignedUrlService to validate
//            if (!_signedUrlService.ValidateSignedUrl(decodedFile, expiresUnix, decodedSignature))
//            {
//                _logger.LogWarning("Invalid or expired signature for file: {DecodedFile}", decodedFile);
//                return Unauthorized("Invalid or expired signature.");
//            }

//            var fullPath = _filePathBuilder.CombineWithUploadPath(decodedFile);
//            if (!System.IO.File.Exists(fullPath))
//            {
//                _logger.LogWarning("File not found at path: {FullPath} for requested file: {DecodedFile}", fullPath, decodedFile);
//                return NotFound("File not found.");
//            }

//            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
//            var contentType = "application/octet-stream";
//            var fileName = Path.GetFileName(fullPath);

//            _logger.LogInformation("Successfully serving file: {FileName} from path: {FullPath}", fileName, fullPath);
//            return File(stream, contentType, fileName);
//        }
//    }
//}