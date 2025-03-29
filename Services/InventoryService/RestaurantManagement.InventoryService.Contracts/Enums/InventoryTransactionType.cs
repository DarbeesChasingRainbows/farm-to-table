namespace RestaurantManagement.InventoryService.Contracts.Enums
{
    public enum InventoryTransactionType
    {
        Received = 1,
        Consumed = 2,
        Transferred = 3,
        Adjusted = 4,
        Wasted = 5,
        Reserved = 6,
        Released = 7
    }
}
