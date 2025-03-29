using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class TransferInventoryCommand
    {
        public List<TransferInventoryItemCommand> Items { get; set; }
        public DateTime TransferDate { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
    }

    public class TransferInventoryItemCommand
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string SourceLocationId { get; set; }
        public string DestinationLocationId { get; set; }
        public List<string> BatchIds { get; set; }
    }

    public class TransferInventoryResult
    {
        public string TransactionId { get; set; }
        public bool Success { get; set; }
        public List<UnavailableItem> UnavailableItems { get; set; }
        public string ErrorMessage { get; set; }
    }
}
