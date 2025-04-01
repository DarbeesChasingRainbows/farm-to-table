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

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryTransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryTransactionsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResultDto<InventoryTransactionSummaryDto>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetInventoryTransactions(
        [FromQuery] GetInventoryTransactionsQuery query
    )
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InventoryTransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInventoryTransaction(string id)
    {
        var query = new GetInventoryTransactionQuery { TransactionId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("receive")]
    [ProducesResponseType(typeof(ReceiveInventoryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReceiveInventory([FromBody] ReceiveInventoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("consume")]
    [ProducesResponseType(typeof(ConsumeInventoryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConsumeInventory([FromBody] ConsumeInventoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("transfer")]
    [ProducesResponseType(typeof(TransferInventoryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TransferInventory([FromBody] TransferInventoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("adjust")]
    [ProducesResponseType(typeof(AdjustInventoryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AdjustInventory([FromBody] AdjustInventoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("waste")]
    [ProducesResponseType(typeof(RecordWasteResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecordWaste([FromBody] RecordWasteCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}

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
    [ProducesResponseType(typeof(PagedResultDto<CountSheetDto>), StatusCodes.Status200OK)]
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
    public async Task<IActionResult> GenerateCountSheet(
        [FromBody] GenerateCountSheetCommand command
    )
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
    public async Task<IActionResult> RecordCounts(string id, [FromBody] RecordCountsCommand command)
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

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<LocationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations([FromQuery] GetLocationsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocation(string id)
    {
        var query = new GetLocationQuery { LocationId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateInventoryLocationResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLocation(
        [FromBody] CreateInventoryLocationCommand command
    )
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetLocation), new { id = result.LocationId }, result);
    }
}

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public BatchesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<BatchDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBatches([FromQuery] GetBatchesQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BatchDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBatch(string id)
    {
        var query = new GetBatchQuery { BatchId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBatchResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBatch([FromBody] CreateBatchCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetBatch), new { id = result.BatchId }, result);
    }
}

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
    [ProducesResponseType(typeof(VendorDto), StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(PagedResultDto<VendorItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVendorItems(
        string id,
        [FromQuery] GetVendorItemsQuery query
    )
    {
        if (id != query.VendorId)
        {
            return BadRequest("ID in URL does not match ID in the query parameters");
        }

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

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
    public async Task<IActionResult> GetReorderSuggestions(
        [FromQuery] GetReorderSuggestionsQuery query
    )
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("generate")]
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

namespace RestaurantManagement.InventoryService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReservations(
        [FromQuery] GetInventoryReservationsQuery query
    )
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("reserve")]
    [ProducesResponseType(typeof(ReserveInventoryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReserveInventory([FromBody] ReserveInventoryCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("release")]
    [ProducesResponseType(typeof(ReleaseReservationResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReleaseReservation(
        [FromBody] ReleaseReservationCommand command
    )
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}

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

    [HttpGet("value")]
    [ProducesResponseType(typeof(InventoryValueDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryValue([FromQuery] GetInventoryValueQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("usage")]
    [ProducesResponseType(typeof(InventoryUsageReportDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryUsage([FromQuery] GetInventoryUsageQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("expiring")]
    [ProducesResponseType(typeof(ExpiringInventoryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpiringInventory(
        [FromQuery] GetExpiringInventoryQuery query
    )
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("alerts")]
    [ProducesResponseType(typeof(PagedResultDto<InventoryAlertDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryAlerts([FromQuery] GetInventoryAlertsQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("generate")]
    [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateReport([FromBody] GenerateInventoryReportQuery query)
    {
        var result = await _mediator.Send(query);

        return File(
            result,
            GetContentType(query.Format),
            $"Inventory_{query.ReportType}_{DateTime.Now:yyyyMMdd}.{GetFileExtension(query.Format)}"
        );
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
