namespace RestaurantManagement.InventoryService.Contracts.Enums
{
    public enum VarianceReasonCode
    {
        CountingError = 1,
        Theft = 2,
        Spoilage = 3,
        Breakage = 4,
        SystemError = 5,
        MissingTransaction = 6,
        MislabeledItem = 7,
        RecordingError = 8,
        UnitOfMeasureConversion = 9,
        LocationError = 10,
        Other = 99
    }
}
