using HotChocolate;
using HotChocolate.Types;
using MediatR;
using RestaurantManagement.InventoryService.Contracts.Commands;

namespace RestaurantManagement.InventoryService.API.GraphQL.Types;

[ExtendObjectType(typeof(Mutation))]
public class WasteMutation
{
    [GraphQLDescription("Record waste for inventory items")]
    public async Task<RecordWasteResult> RecordWaste(
        [Service] IMediator mediator,
        IReadOnlyList<WasteItemInput> items,
        string wasteReason,
        string? notes = null
    )
    {
        var command = new RecordWasteCommand
        {
            Items = items
                .Select(
                    i => new RestaurantManagement.InventoryService.Contracts.Commands.WasteItemCommand
                    {
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        LocationId = i.LocationId,
                        BatchId = i.BatchId,
                        CostPerUnit = i.CostPerUnit
                    }
                )
                .ToList(),
            WasteDate = DateTime.UtcNow,
            WasteReason = wasteReason,
            Notes = notes,
            UserId = "graphql_user" // In a real system, this would come from authentication
        };

        return await mediator.Send(command);
    }
}

[ExtendObjectType(typeof(Mutation))]
public class AdjustMutation
{
    [GraphQLDescription("Adjust inventory quantities")]
    public async Task<AdjustInventoryResult> AdjustInventory(
        [Service] IMediator mediator,
        IReadOnlyList<AdjustInventoryItemInput> items,
        string? notes = null
    )
    {
        var command = new AdjustInventoryCommand
        {
            Items = items
                .Select(
                    i => new RestaurantManagement.InventoryService.Contracts.Commands.AdjustInventoryItemCommand
                    {
                        ItemId = i.ItemId,
                        LocationId = i.LocationId,
                        CurrentQuantity = i.CurrentQuantity,
                        NewQuantity = i.NewQuantity,
                        UserId = "graphql_user" // In a real system, this would come from authentication
                    }
                )
                .ToList(),
            AdjustmentDate = DateTime.UtcNow,
            Notes = notes
        };

        return await mediator.Send(command);
    }
}
