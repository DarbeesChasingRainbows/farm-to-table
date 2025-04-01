using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.InventoryService.Contracts.Commands;
using RestaurantManagement.InventoryService.Contracts.DTOs;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountSheetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CountSheetsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<CountSheetSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCountSheets([FromQuery] GetCountSheetsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CountSheetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCountSheet(string id)
    {
        var query = new GetCountSheetQuery { CountSheetId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(GenerateCountSheetResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateCountSheet([FromBody] GenerateCountSheetCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("{id}/record-counts")]
    [ProducesResponseType(typeof(RecordCountsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RecordCounts(
        string id,
        [FromBody] RecordCountsCommand command
    )
    {
        if (id != command.CountSheetId)
        {
            return BadRequest("ID in URL does not match ID in the request body");
        }

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("{id}/approve-variances")]
    [ProducesResponseType(typeof(ApproveVariancesResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveVariances(
        string id,
        [FromBody] ApproveVariancesCommand command
    )
    {
        if (id != command.CountSheetId)
        {
            return BadRequest("ID in URL does not match ID in the request body");
        }

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
