using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class ReleaseReservationCommand
    {
        public string ReservationId { get; set; }
        public string ReferenceId { get; set; }
        public List<ReleaseReservationItemCommand> Items { get; set; }
    }

    public class ReleaseReservationItemCommand
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public string LocationId { get; set; }
    }

    public class ReleaseReservationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
