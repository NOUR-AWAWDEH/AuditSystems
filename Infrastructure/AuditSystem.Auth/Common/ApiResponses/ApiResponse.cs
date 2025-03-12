namespace AuditSystem.Auth.Common.ApiResponses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public object? Errors { get; set; }

        public ApiResponse(bool success, string message, T? data = default, object? errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors;
        }
    }

    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse(bool success, string message, object? errors = null)
            : base(success, message, null, errors)
        {
        }
    }
}