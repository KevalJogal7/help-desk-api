using HelpDesk.Domain.Entities;
using HelpDesk.Repositories.Interfaces;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.TicketDTOs;
using HelpDesk.Services.Enums;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _repository;
    private readonly IAuthService _authService;

    public TicketService(ITicketRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }


    public async Task<BaseResponse<PagedResponse<TicketResponse>>> GetList(TicketRequest request)
    {
        IQueryable<Ticket> query = _repository.GetList();
        string email = _authService.Email;
        RoleEnum role = _authService.Role;

        if(role == RoleEnum.USER)
        {
            query = query.Where(t => t.CreatedByNavigation.Email == email);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();

            query = query.Where(t =>
                t.TicketNumber.ToLower().Contains(search) ||
                t.Title.ToLower().Contains(search) ||
                t.Description.ToLower().Contains(search) || 
                t.CreatedByNavigation.FirstName.ToLower().Contains(search) || 
                t.CreatedByNavigation.LastName.ToLower().Contains(search));
        }

        if (request.Category > 0)
        {
            query = query.Where(t => t.CategoryId == request.Category);
        }

        if (request.SubCategory > 0)
        {
            query = query.Where(t => t.SubCategoryId == request.SubCategory);
        }

        if (request.Status > 0)
        {
            query = query.Where(t => t.StatusId == request.Status);
        }

        if (request.Priority > 0)
        {
            query = query.Where(t => t.PriorityId == request.Priority);
        }

        query = query.ApplySorting(request.SortBy, request.SortDirection == "desc");

        var totalCount = await query.CountAsync();

        var tickets = await query
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        List<TicketResponse> records = new();

        foreach (var ticket in tickets)
        {
            records.Add(new TicketResponse
            {
                TicketId = ticket.TicketId,
                TicketNumber = ticket.TicketNumber,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedBy = $"{ticket.CreatedByNavigation.FirstName} {ticket.CreatedByNavigation.LastName}",
                CreatedOn = ticket.CreatedOn,
                Priority = ticket.Priority.PriorityName,
                Status = ticket.Status.StatusName,
                Category = ticket.Category.Name,
                SubCategory = ticket.SubCategory.Name
            });
        }

        PagedResponse<TicketResponse> response = new PagedResponse<TicketResponse>()
        {
            Items = records,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
        };

        return ResponseFactory.Success(
            response,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }

    public async Task<BaseResponse<List<DropdownOption>>> GetCategoryList()
    {
        List<Category> query = await _repository.GetCategoryList();

        List<DropdownOption> records = new();

        foreach (var category in query)
        {
            records.Add(new DropdownOption
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive
            });
        }

        return ResponseFactory.Success(
            records,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }

    public async Task<BaseResponse<List<SubCategoryResponse>>> GetSubCategoryList()
    {
        List<SubCategory> query = await _repository.GetSubCategoryList();

        List<SubCategoryResponse> records = new();

        foreach (var category in query)
        {
            records.Add(new SubCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                IsActive = category.IsActive,
                CategoryId = category.CategoryId
            });
        }

        return ResponseFactory.Success(
            records,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }

    public async Task<BaseResponse<List<DropdownOption>>> GetStatusList()
    {
        List<TicketStatus> query = await _repository.GetStatusList();

        List<DropdownOption> records = new();

        foreach (var status in query)
        {
            records.Add(new DropdownOption
            {
                Id = status.StatusId,
                Name = status.StatusName,
                IsActive = status.IsActive
            });
        }

        return ResponseFactory.Success(
            records,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }

    public async Task<BaseResponse<List<DropdownOption>>> GetPriorityList()
    {
        List<TicketPriority> query = await _repository.GetPriorityList();

        List<DropdownOption> records = new();

        foreach (var priority in query)
        {
            records.Add(new DropdownOption
            {
                Id = priority.PriorityId,
                Name = priority.PriorityName,
                IsActive = priority.IsActive
            });
        }

        return ResponseFactory.Success(
            records,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }

}