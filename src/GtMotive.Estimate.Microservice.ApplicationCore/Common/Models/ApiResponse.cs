namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.Models
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
            Success = true;
            Message = string.Empty;
        }

        public ApiResponse(string errorMessage)
        {
            Data = default;
            Success = false;
            Message = errorMessage;
        }

        public T Data { get; }
        public bool Success { get; }
        public string Message { get; }
    }

    public class ApiResponse
    {
        public ApiResponse(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        public string Message { get; }

        public static ApiResponse SuccessResult() => new(true);
        public static ApiResponse ErrorResult(string message) => new(false, message);
    }
}
