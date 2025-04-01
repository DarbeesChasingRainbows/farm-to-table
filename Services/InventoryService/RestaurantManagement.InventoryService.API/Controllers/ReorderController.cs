using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.InventoryService.Contracts.Commands;
using RestaurantManagement.InventoryService.Contracts.DTOs;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReorderController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReorderController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet("suggestions")]
    [ProducesResponseType(typeof(List<ReorderSuggestionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReorderSuggestions([FromQuery] GetReorderSuggestionsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("generate-suggestions")]
    [ProducesResponseType(typeof(GenerateReorderSuggestionsResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateReorderSuggestions(
        [FromBody] GenerateReorderSuggestionsCommand command
    )
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("approve")]
    [ProducesResponseType(typeof(ApproveReorderResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ApproveReorder([FromBody] ApproveReorderCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
