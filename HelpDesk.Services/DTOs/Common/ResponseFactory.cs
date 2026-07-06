namespace HelpDesk.Services.DTOs.Common;

public static class ResponseFactory
{
    public static BaseResponse<T> Success<T>(
        T data,
        string message = "Success",
        int statusCode = 200)
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            StatusCode = statusCode
        };
    }

    public static BaseResponse<T> Failure<T>(
        string message,
        int statusCode = 400,
        List<string>? errors = null)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            StatusCode = statusCode
        };
    }
}