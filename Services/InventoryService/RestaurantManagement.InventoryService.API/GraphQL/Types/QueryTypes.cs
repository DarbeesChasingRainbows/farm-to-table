using HotChocolate;
using HotChocolate.Types;
using MediatR;
using RestaurantManagement.InventoryService.Contracts.DTOs;
using RestaurantManagement.InventoryService.Contracts.Queries;

namespace RestaurantManagement.InventoryService.API.GraphQL.Types;

[ExtendObjectType(typeof(Query))]
public class InventoryQuery
{
    [GraphQLDescription("Get inventory items with optional filtering")]
    public async Task<PagedResultDto<InventoryItemDto>> GetInventoryItems(
        [Service] IMediator mediator,
        string? search = null,
        string? category = null,
        string? locationId = null,
        bool includeInactive = false,
        bool includeZeroStock = true,
        bool belowThreshold = false,
        int page = 1,
        int pageSize = 20
    )
    {
        var query = new GetInventoryItemsQuery
        {
            Search = search,
            Category = category,
            LocationId = locationId,
            IncludeInactive = includeInactive,
            IncludeZeroStock = includeZeroStock,
            BelowThreshold = belowThreshold,
            Page = page,
            PageSize = pageSize
        };

        return await mediator.Send(query);
    }
}

[ExtendObjectType(typeof(Query))]
public class ItemQuery
{
    [GraphQLDescription("Get inventory item details by ID")]
    public async Task<InventoryItemDetailDto?> GetInventoryItem(
        [Service] IMediator mediator,
        string id,
        bool includeStockLevels = true,
        bool includeBatches = false,
        bool includeTransactions = false
    )
    {
        var query = new GetInventoryItemQuery
        {
            ItemId = id,
            IncludeStockLevels = includeStockLevels,
            IncludeBatches = includeBatches,
            IncludeTransactions = includeTransactions
        };

        return await mediator.Send(query);
    }
}

[ExtendObjectType(typeof(Query))]
public class LocationQuery
{
    [GraphQLDescription("Get all inventory locations")]
    public async Task<PagedResultDto<LocationDto>> GetLocations(
        [Service] IMediator mediator,
        IReadOnlyList<string>? locationTypes = null,
        bool includeInactive = false,
        string? search = null,
        int page = 1,
        int pageSize = 20
    )
    {
        var query = new GetLocationsQuery
        {
            LocationTypes = locationTypes?.ToList(),
            IncludeInactive = includeInactive,
            Search = search,
            Page = page,
            PageSize = pageSize
        };

        return await mediator.Send(query);
    }
}

[ExtendObjectType(typeof(Query))]
public class TransactionQuery
{
    [GraphQLDescription("Get inventory transactions with filtering options")]
    public async Task<PagedResultDto<InventoryTransactionSummaryDto>> GetInventoryTransactions(
        [Service] IMediator mediator,
        string? itemId = null,
        string? locationId = null,
        IReadOnlyList<string>? transactionTypes = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? referenceId = null,
        int page = 1,
        int pageSize = 20
    )
    {
        var query = new GetInventoryTransactionsQuery
        {
            ItemId = itemId,
            LocationId = locationId,
            TransactionTypes = transactionTypes?.ToList(),
            StartDate = startDate,
            EndDate = endDate,
            ReferenceId = referenceId,
            Page = page,
            PageSize = pageSize
        };

        return await mediator.Send(query);
    }
}

[ExtendObjectType(typeof(Query))]
public class StockQuery
{
    [GraphQLDescription("Get stock levels for inventory items")]
    public async Task<PagedResultDto<StockLevelDto>> GetStockLevels(
        [Service] IMediator mediator,
        IReadOnlyList<string>? itemIds = null,
        string? locationId = null,
        bool belowThreshold = false,
        bool includeZeroStock = true,
        int page = 1,
        int pageSize = 20
    )
    {
        var query = new GetStockLevelsQuery
        {
            ItemIds = itemIds?.ToList(),
            LocationId = locationId,
            BelowThreshold = belowThreshold,
            IncludeZeroStock = includeZeroStock,
            Page = page,
            PageSize = pageSize
        };

        return await mediator.Send(query);
    }
}

[ExtendObjectType(typeof(Query))]
public class ValueQuery
{
    [GraphQLDescription("Get inventory value with optional filtering")]
    public async Task<InventoryValueDto> GetInventoryValue(
        [Service] IMediator mediator,
        IReadOnlyList<string>? locationIds = null,
        IReadOnlyList<string>? categoryIds = null,
        DateTime? asOfDate = null
    )
    {
        var query = new GetInventoryValueQuery
        {
            LocationIds = locationIds?.ToList(),
            CategoryIds = categoryIds?.ToList(),
            AsOfDate = asOfDate
        };

        return await mediator.Send(query);
    }
}
