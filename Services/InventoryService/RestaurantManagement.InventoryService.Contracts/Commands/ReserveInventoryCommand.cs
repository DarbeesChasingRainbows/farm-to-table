using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class ReserveInventoryCommand
    {
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public List<ReserveInventoryItemCommand> Items { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class ReserveInventoryItemCommand
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public string LocationId { get; set; }
    }

    public class ReserveInventoryResult
    {
        public string ReservationId { get; set; }
        public bool Success { get; set; }
        public List<UnavailableItem> UnavailableItems { get; set; }
        public string ErrorMessage { get; set; }
    }
}
