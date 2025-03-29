namespace RestaurantManagement.InventoryService.Contracts.Enums
{
    public enum InventoryAlertType
    {
        LowStock = 1,
        OutOfStock = 2,
        ExpirationApproaching = 3,
        ExpirationPassed = 4,
        PriceIncrease = 5,
        LargeVariance = 6,
        UnexpectedMovement = 7,
        ExcessiveWaste = 8,
        ReorderSuggested = 9
    }
}
