namespace HelpDesk.Services.DTOs.Common;

public class PagedResponse<T>
{
    public List<T> Items { get; set; } = new();

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}
