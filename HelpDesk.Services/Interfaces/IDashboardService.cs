
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.DashboardDTOs;

namespace HelpDesk.Services.Interfaces;

public interface IDashboardService
{
   Task<BaseResponse<DashboardResponse>> GetDashboard();
}
