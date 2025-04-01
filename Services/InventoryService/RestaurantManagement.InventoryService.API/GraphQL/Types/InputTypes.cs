using HotChocolate;
using HotChocolate.Types;
using RestaurantManagement.InventoryService.Contracts.DTOs;

namespace RestaurantManagement.InventoryService.API.GraphQL.Types;

[InputType]
public class WasteItemInput
{
    [GraphQLDescription("The ID of the inventory item")]
    public string ItemId { get; set; }

    [GraphQLDescription("The quantity to waste")]
    public double Quantity { get; set; }

    [GraphQLDescription("The ID of the location")]
    public string LocationId { get; set; }

    [GraphQLDescription("The ID of the batch, if applicable")]
    public string? BatchId { get; set; }

    [GraphQLDescription("The cost per unit for the wasted items")]
    public decimal? CostPerUnit { get; set; }
}

[InputType]
public class AdjustInventoryItemInput
{
    [GraphQLDescription("The ID of the inventory item")]
    public string ItemId { get; set; }

    [GraphQLDescription("The ID of the location")]
    public string LocationId { get; set; }

    [GraphQLDescription("The current quantity before adjustment")]
    public double CurrentQuantity { get; set; }

    [GraphQLDescription("The new quantity after adjustment")]
    public double NewQuantity { get; set; }
}
