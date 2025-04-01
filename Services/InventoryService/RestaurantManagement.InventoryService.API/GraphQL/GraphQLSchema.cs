using HotChocolate;
using HotChocolate.Types;

namespace RestaurantManagement.InventoryService.API.GraphQL;

public class Query
{
    // Base query class - all queries will be extended from this
}

public class Mutation
{
    // Base mutation class - all mutations will be extended from this
}

public class GraphQLSchema : Schema
{
    public GraphQLSchema()
    {
        RegisterQueryTypes();
        RegisterMutationTypes();
    }

    private void RegisterQueryTypes()
    {
        // Register all query types
        Services.AddSingleton<InventoryQuery>();
        Services.AddSingleton<ItemQuery>();
        Services.AddSingleton<LocationQuery>();
        Services.AddSingleton<TransactionQuery>();
        Services.AddSingleton<StockQuery>();
        Services.AddSingleton<ValueQuery>();
    }

    private void RegisterMutationTypes()
    {
        // Register all mutation types
        Services.AddSingleton<WasteMutation>();
        Services.AddSingleton<AdjustMutation>();
    }
}
