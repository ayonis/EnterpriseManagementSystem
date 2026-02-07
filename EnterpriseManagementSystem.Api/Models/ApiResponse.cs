using EnterpriseManagementSystem.Application.Common;

namespace EnterpriseManagementSystem.Api.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IReadOnlyList<string> Errors { get; set; } = Array.Empty<string>();
        public string? ErrorType { get; set; }
    }

    public static class ApiResponse
    {
        public static ApiResponse<T> Success<T>(T data, string? message = null) => new()
            {
                IsSuccess = true,
                Data = data,
                ErrorType = ErrorType.None.ToString(),
                Message = message,
                Errors = Array.Empty<string>()
        };

        public static ApiResponse<T> Failure<T>(string errorMessage, ErrorType errorType = ErrorType.Business, IEnumerable<string> errors = null) => new()
            {
                IsSuccess = false,
                Data = default,
                ErrorType = errorType.ToString() ,
                Message = errorMessage ?? "Operation failed",
                Errors = errors != null ? errors.ToList() : new List<string>(),
        };
    }

}
