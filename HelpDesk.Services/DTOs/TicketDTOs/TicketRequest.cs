using HelpDesk.Services.DTOs.Common;

namespace HelpDesk.Services.DTOs.TicketDTOs;

public class TicketRequest : PagedRequest
{
    public int Category { get; set; } = 0;

    public int SubCategory { get; set; } = 0;

    public int Status { get; set; } = 0;

    public int Priority { get; set; } = 0;
}
