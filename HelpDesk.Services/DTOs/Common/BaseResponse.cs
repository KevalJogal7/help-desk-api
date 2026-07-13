namespace HelpDesk.Services.DTOs.Common;

public class BaseResponse<T>
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public T? Data { get; set; }

    public List<string>? Errors { get; set; }

    public int StatusCode { get; set; }
}
