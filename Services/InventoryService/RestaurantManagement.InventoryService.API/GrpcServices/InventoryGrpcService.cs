using Grpc.Core;
using MediatR;
using RestaurantManagement.InventoryService.API.GrpcServices;
using RestaurantManagement.InventoryService.Application.Queries.InventoryItems;
using RestaurantManagement.InventoryService.Contracts.Commands;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.Services;

public class InventoryGrpcService : GrpcServices.InventoryService.InventoryServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<InventoryGrpcService> _logger;

    public InventoryGrpcService(IMediator mediator, ILogger<InventoryGrpcService> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<InventoryItemResponse> GetInventoryItem(
        GetInventoryItemRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation(
            "Received gRPC request for inventory item with ID: {ItemId}",
            request.ItemId
        );

        try
        {
            var query = new GetInventoryItemQuery { ItemId = request.ItemId };
            var result = await _mediator.Send(query);

            if (result == null)
            {
                throw new RpcException(
                    new Status(
                        StatusCode.NotFound,
                        $"Inventory item with ID {request.ItemId} not found"
                    )
                );
            }

            return new InventoryItemResponse
            {
                Id = result.Id,
                Name = result.Name,
                Sku = result.SKU,
                Category = result.Category,
                UnitOfMeasure = result.UnitOfMeasure
            };
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            _logger.LogError(
                ex,
                "Error retrieving inventory item with ID: {ItemId}",
                request.ItemId
            );
            throw new RpcException(new Status(StatusCode.Internal, "An internal error occurred"));
        }
    }

    public override async Task<CheckAvailabilityResponse> CheckAvailability(
        CheckAvailabilityRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation(
            "Received gRPC request to check availability for {ItemCount} items at location: {LocationId}",
            request.Items.Count,
            request.LocationId
        );

        try
        {
            var query = new CheckInventoryAvailabilityQuery
            {
                LocationId = request.LocationId,
                Items = request
                    .Items.Select(i => new InventoryItemCheck
                    {
                        ItemId = i.ItemId,
                        RequiredQuantity = i.RequiredQuantity
                    })
                    .ToList()
            };

            var result = await _mediator.Send(query);

            var response = new CheckAvailabilityResponse { AllAvailable = true };

            if (result.UnavailableItems != null && result.UnavailableItems.Count > 0)
            {
                response.AllAvailable = false;
                foreach (var unavailableItem in result.UnavailableItems)
                {
                    response.UnavailableItems.Add(
                        new UnavailableItem
                        {
                            ItemId = unavailableItem.ItemId,
                            RequiredQuantity = unavailableItem.RequestedQuantity,
                            AvailableQuantity = unavailableItem.AvailableQuantity
                        }
                    );
                }
            }

            return response;
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            _logger.LogError(ex, "Error checking inventory availability");
            throw new RpcException(new Status(StatusCode.Internal, "An internal error occurred"));
        }
    }

    public override async Task<ReceiveInventoryResponse> ReceiveInventory(
        ReceiveInventoryRequest request,
        ServerCallContext context
    )
    {
        _logger.LogInformation(
            "Received gRPC request to receive inventory for PO: {PurchaseOrderId} with {ItemCount} items",
            request.PurchaseOrderId,
            request.Items.Count
        );

        try
        {
            var command = new ReceiveInventoryCommand
            {
                PurchaseOrderId = request.PurchaseOrderId,
                ReceivedDate = DateTime.UtcNow,
                UserId = context.GetHttpContext()?.User?.Identity?.Name ?? "gRPC_System",
                Items = request
                    .Items.Select(i => new ReceiveInventoryItemCommand
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        LocationId = i.LocationId
                    })
                    .ToList()
            };

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, result.ErrorMessage));
            }

            var response = new ReceiveInventoryResponse { TransactionId = result.TransactionId };

            foreach (var updatedLevel in result.UpdatedStockLevels)
            {
                response.UpdatedStockLevels.Add(
                    new UpdatedStockLevel
                    {
                        ItemId = updatedLevel.ItemId,
                        LocationId = updatedLevel.LocationId,
                        CurrentQuantity = updatedLevel.CurrentQuantity
                    }
                );
            }

            return response;
        }
        catch (Exception ex) when (ex is not RpcException)
        {
            _logger.LogError(ex, "Error receiving inventory");
            throw new RpcException(new Status(StatusCode.Internal, "An internal error occurred"));
        }
    }
}
