using System;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class DiscontinueInventoryItemCommand
    {
        public string Id { get; set; }
        public string Reason { get; set; }
    }

    public class DiscontinueInventoryItemResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
