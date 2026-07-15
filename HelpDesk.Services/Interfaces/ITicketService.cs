using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.TicketDTOs;

namespace HelpDesk.Services.Interfaces;

public interface ITicketService
{
    Task<BaseResponse<PagedResponse<TicketResponse>>> GetList(TicketRequest request);
    Task<BaseResponse<TicketResponse>> GetTicketById(Guid id);
    Task<BaseResponse<object>> UpsertTicket(UpsertTicketRequest request);
    Task<BaseResponse<object>> DeleteTicket(Guid id);
    Task<BaseResponse<List<DropdownOption>>> GetCategoryList();
    Task<BaseResponse<List<SubCategoryResponse>>> GetSubCategoryList();
    Task<BaseResponse<List<DropdownOption>>> GetStatusList();
    Task<BaseResponse<List<DropdownOption>>> GetPriorityList();
    Task<BaseResponse<object>> AssignTicket(TicketAssignRequest request);
}
