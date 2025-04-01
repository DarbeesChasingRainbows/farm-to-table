using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.InventoryService.Contracts.Commands;
using RestaurantManagement.InventoryService.Contracts.DTOs;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendorsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<VendorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVendors([FromQuery] GetVendorsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VendorDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVendor(string id)
    {
        var query = new GetVendorQuery { VendorId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id}/items")]
    [ProducesResponseType(typeof(List<VendorItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVendorItems(string id)
    {
        var query = new GetVendorItemsQuery { VendorId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
