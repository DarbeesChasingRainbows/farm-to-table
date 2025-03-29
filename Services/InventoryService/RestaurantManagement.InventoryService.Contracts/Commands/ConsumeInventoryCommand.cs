using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class ConsumeInventoryCommand
    {
        public List<ConsumeInventoryItemCommand> Items { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceId { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
    }

    public class ConsumeInventoryItemCommand
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public string LocationId { get; set; }
        public List<string> BatchIds { get; set; }
    }

    public class ConsumeInventoryResult
    {
        public string TransactionId { get; set; }
        public List<ConsumedItem> ConsumedItems { get; set; }
        public bool Success { get; set; }
        public List<UnavailableItem> UnavailableItems { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ConsumedItem
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
    }

    public class UnavailableItem
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public double RequestedQuantity { get; set; }
        public double AvailableQuantity { get; set; }
        public string LocationId { get; set; }
    }
}
