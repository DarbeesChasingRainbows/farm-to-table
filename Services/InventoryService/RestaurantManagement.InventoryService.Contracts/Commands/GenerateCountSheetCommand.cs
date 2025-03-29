using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class GenerateCountSheetCommand
    {
        public string LocationId { get; set; }
        public List<string> CategoryIds { get; set; }
        public string RequestedBy { get; set; }
        public DateTime CountDate { get; set; }
        public string Notes { get; set; }
    }

    public class GenerateCountSheetResult
    {
        public string CountSheetId { get; set; }
        public int ItemCount { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
