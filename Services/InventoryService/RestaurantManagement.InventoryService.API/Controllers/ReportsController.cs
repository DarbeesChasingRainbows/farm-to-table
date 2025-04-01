using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.InventoryService.Contracts.Commands;
using RestaurantManagement.InventoryService.Contracts.DTOs;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("inventory-value")]
    [ProducesResponseType(typeof(InventoryValueReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryValue([FromQuery] GetInventoryValueQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("inventory-usage")]
    [ProducesResponseType(typeof(InventoryUsageReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryUsage([FromQuery] GetInventoryUsageQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("expiring-inventory")]
    [ProducesResponseType(typeof(ExpiringInventoryReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpiringInventory(
        [FromQuery] GetExpiringInventoryQuery query
    )
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("inventory-alerts")]
    [ProducesResponseType(typeof(InventoryAlertsReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryAlerts([FromQuery] GetInventoryAlertsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateReportCommand command)
    {
        var result = await _mediator.Send(command);

        if (result == null || result.FileContent == null)
        {
            return BadRequest("Failed to generate report");
        }

        var contentType = GetContentType(command.Format);
        var fileExtension = GetFileExtension(command.Format);
        var fileName = $"{command.ReportType}_{DateTime.UtcNow:yyyyMMdd}.{fileExtension}";

        return File(result.FileContent, contentType, fileName);
    }

    private string GetContentType(string format)
    {
        return format.ToLower() switch
        {
            "pdf" => "application/pdf",
            "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "csv" => "text/csv",
            _ => "application/octet-stream"
        };
    }

    private string GetFileExtension(string format)
    {
        return format.ToLower() switch
        {
            "pdf" => "pdf",
            "excel" => "xlsx",
            "csv" => "csv",
            _ => "bin"
        };
    }
}
