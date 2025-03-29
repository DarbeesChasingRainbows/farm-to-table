namespace RestaurantManagement.InventoryService.Contracts.Enums
{
    public enum CostingMethod
    {
        FIFO = 1,
        LIFO = 2,
        WeightedAverage = 3,
        SpecificIdentification = 4,
        StandardCost = 5,
        LastPurchasePrice = 6
    }
}
