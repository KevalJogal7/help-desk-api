namespace HelpDesk.Services.DTOs.TicketDTOs;

public class TicketRequest
{
    public int Page { get; set; } = 0;

    public int PageSize { get; set; } = 10;

    public int Category { get; set; } = 0;

    public int SubCategory { get; set; } = 0;

    public int Status { get; set; } = 0;

    public int Priority { get; set; } = 0;

    public string? Search { get; set; }

    public string? SortBy { get; set; } = "CreatedOn";

    public string? SortDirection { get; set; } = "desc";
}
