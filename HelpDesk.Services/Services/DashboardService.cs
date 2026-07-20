namespace HelpDesk.Services.Services;

using System.Data;
using System.Text.Json;
using Dapper;
using HelpDesk.Services.Constants;
using HelpDesk.Services.DTOs.Common;
using HelpDesk.Services.DTOs.DashboardDTOs;
using HelpDesk.Services.Interfaces;
using Microsoft.AspNetCore.Http;

public class DashboardService : IDashboardService
{
    private readonly IDbConnection _connection;
    public DashboardService(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<BaseResponse<DashboardResponse>> GetDashboard()
    {
        var json = await _connection.QuerySingleAsync<string>("SELECT get_dashboard();");

        var result = JsonSerializer.Deserialize<DashboardResponse>(json, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });

        return ResponseFactory.Success(
            result,
            Messages.General.Success,
            StatusCodes.Status200OK
        );
    }

}