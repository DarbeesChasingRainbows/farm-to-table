// File: RestaurantManagement.Common/Models/ApiResponse.cs
namespace RestaurantManagement.Common.Models;

/// <summary>
/// Standard API response wrapper for consistent API responses across all services.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T Data { get; }
    public IEnumerable<string> Errors { get; }

    private ApiResponse(bool success, string message, T data, IEnumerable<string> errors)
    {
        Success = success;
        Message = message;
        Data = data;
        Errors = errors ?? new List<string>();
    }

    public static ApiResponse<T> Succeed(T data, string message = null)
    {
        return new ApiResponse<T>(true, message ?? "Operation completed successfully", data, null);
    }

    public static ApiResponse<T> Fail(string message, IEnumerable<string> errors = null)
    {
        return new ApiResponse<T>(false, message, default, errors);
    }
}
