namespace HelpDesk.Services.DTOs.DashboardDTOs;

using HelpDesk.Services.DTOs.TicketDTOs;

public class DashboardResponse
{
    public DashboardStats Stats { get; set; } = new();
    public List<DashboardStatusCount> StatusBreakdown { get; set; } = new();
    public List<DashboardPriorityCount> PriorityBreakdown { get; set; } = new();
    public List<TicketResponse> HighPriorityTickets { get; set; } = new();
    public List<TicketResponse> RecentTickets { get; set; } = new();
}

public class DashboardStats
{
    public int TotalTickets { get; set; }
    public int OpenTickets { get; set; }
    public int ResolvedToday { get; set; }
    public int OverdueTickets { get; set; }
}

public class DashboardStatusCount
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class DashboardPriorityCount
{
    public string Priority { get; set; } = string.Empty;
    public int Count { get; set; }
}