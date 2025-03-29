namespace RestaurantManagement.InventoryService.Domain.Enums
{
    public enum WasteReasonCode
    {
        Expired = 1,
        Spoiled = 2,
        Damaged = 3,
        QualityIssue = 4,
        PreparationError = 5,
        CustomerReturn = 6,
        Contamination = 7,
        Overproduction = 8,
        SpillageOrBreakage = 9,
        Training = 10,
        Testing = 11,
        Other = 99
    }
}
