using Ardalis.Result;
using AuditSystem.Contract.Common;
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
        // Map errors to ApiErrorResponse
        var errors = result.Errors.Select(error => new ApiErrorResponse(error)).ToList();

        switch (result.Status)
        {
            case ResultStatus.Invalid:
                var validationErrors = result
                    .ValidationErrors
                    .Select(validation => new ApiErrorResponse(validation.ErrorMessage))
                    .ToList();

                // Correctly pass IEnumerable<ApiErrorResponse> to the BadRequest method
                return new BadRequestObjectResult(ApiResponse.BadRequest(validationErrors));

            case ResultStatus.NotFound:
                var notFoundResponse = ApiResponse.NotFound(errors);
                return new ObjectResult(notFoundResponse) { StatusCode = notFoundResponse.StatusCode };

            case ResultStatus.Forbidden:
                return new ForbidResult();

            case ResultStatus.Unauthorized:
                var unauthorizedResponse = ApiResponse.Unauthorized(errors); // Passing List<ApiErrorResponse>
                return new UnauthorizedObjectResult(unauthorizedResponse);

            default:
                // Default to BadRequest
                var badRequestResponse = ApiResponse.BadRequest(errors); // Passing List<ApiErrorResponse>
                return new BadRequestObjectResult(badRequestResponse);
        }
    }



}