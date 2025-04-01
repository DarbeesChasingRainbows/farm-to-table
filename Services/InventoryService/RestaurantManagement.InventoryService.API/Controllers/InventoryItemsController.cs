using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.InventoryService.Application.Commands.InventoryItems;
using RestaurantManagement.InventoryService.Application.Queries.InventoryItems;
using RestaurantManagement.InventoryService.Contracts.Commands;
using RestaurantManagement.InventoryService.Contracts.DTOs;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryItemsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<InventoryItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryItems([FromQuery] GetInventoryItemsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InventoryItemDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInventoryItem(string id)
    {
        var query = new GetInventoryItemQuery { ItemId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateInventoryItemResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateInventoryItem(
        [FromBody] CreateInventoryItemCommand command
    )
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetInventoryItem), new { id = result.ItemId }, result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateInventoryItemResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateInventoryItem(
        string id,
        [FromBody] UpdateInventoryItemCommand command
    )
    {
        if (id != command.Id)
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

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(DiscontinueInventoryItemResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DiscontinueInventoryItem(string id, [FromBody] string reason)
    {
        var command = new DiscontinueInventoryItemCommand { Id = id, Reason = reason };

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
