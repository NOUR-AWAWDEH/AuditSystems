using Ardalis.Result;
using AuditSystem.Host.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AuditSystem.Host.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result) =>
        result.IsSuccess ?
            new OkObjectResult(ApiResponse.Ok(result.SuccessMessage)) :
            result.ToHttpNonSuccessResult();

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsCreated())
        {
            return new CreatedResult(
                result.Location, ApiResponse<T>.Created(result.Value));
        }

        if (result.IsOk())
        {
            return new OkObjectResult(
                ApiResponse<T>.Ok(result.Value, result.SuccessMessage));
        }

        return result.ToHttpNonSuccessResult();
    }

    private static IActionResult ToHttpNonSuccessResult(this Ardalis.Result.IResult result)
    {
        var errors = result.Errors.Select(error => new ApiErrorResponse(error)).ToList();

        switch (result.Status)
        {
            case ResultStatus.Invalid:

                var validationErrors = result
                    .ValidationErrors
                    .Select(validation => new ApiErrorResponse(validation.ErrorMessage));

                return new BadRequestObjectResult(ApiResponse.BadRequest(validationErrors));

            case ResultStatus.NotFound:
                return new NotFoundObjectResult(ApiResponse.NotFound(errors));

            case ResultStatus.Forbidden:
                return new ForbidResult();

            case ResultStatus.Unauthorized:
                return new UnauthorizedObjectResult(ApiResponse.Unauthorized(errors));

            default:
                return new BadRequestObjectResult(ApiResponse.BadRequest(errors));
        }
    }
}