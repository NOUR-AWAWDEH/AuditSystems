// Object for error responses
const ApiErrorResponse = {
  message: ''
};

// Non-generic ApiResponse for responses without a result
class ApiResponse {
  constructor(success, statusCode, successMessage = '', errors = []) {
    this.success = success;
    this.statusCode = statusCode;
    this.successMessage = successMessage;
    this.errors = errors;
  }

  // Static factory methods for common responses
  static ok(successMessage = '') {
    return new ApiResponse(true, 200, successMessage);
  }

  static badRequest(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponse(false, 400, undefined, errors);
  }

  static unauthorized(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponse(false, 401, undefined, errors);
  }

  static forbidden(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponse(false, 403, undefined, errors);
  }

  static notFound(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponse(false, 404, undefined, errors);
  }

  static internalServerError(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponse(false, 500, undefined, errors);
  }
}

// Generic ApiResponse for responses with a result
class ApiResponseT extends ApiResponse {
  constructor(success, statusCode, result, successMessage = '', errors = []) {
    super(success, statusCode, successMessage, errors);
    this.result = result;
  }

  // Static factory methods for common responses
  static ok(result, successMessage = '') {
    return new ApiResponseT(true, 200, result, successMessage);
  }

  static created(result, successMessage = '') {
    return new ApiResponseT(true, 201, result, successMessage);
  }

  static badRequest(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponseT(false, 400, undefined, undefined, errors);
  }

  static unauthorized(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponseT(false, 401, undefined, undefined, errors);
  }

  static forbidden(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponseT(false, 403, undefined, undefined, errors);
  }

  static notFound(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponseT(false, 404, undefined, undefined, errors);
  }

  static internalServerError(errorMessage) {
    const errors = Array.isArray(errorMessage)
      ? errorMessage
      : [{ message: errorMessage }];
    return new ApiResponseT(false, 500, undefined, undefined, errors);
  }
}

export { ApiResponse, ApiResponseT, ApiErrorResponse };
