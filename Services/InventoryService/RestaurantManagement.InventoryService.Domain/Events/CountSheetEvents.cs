using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class CountSheetEvent : BaseDomainEvent
    {
        public string CountSheetId { get; }

        protected CountSheetEvent(string countSheetId, string userId)
            : base(userId)
        {
            CountSheetId = countSheetId ?? throw new ArgumentNullException(nameof(countSheetId));
        }
    }

    public class CountSheetCreatedEvent : CountSheetEvent
    {
        public string LocationId { get; }
        public DateTime CountDate { get; }
        public string RequestedBy { get; }

        public CountSheetCreatedEvent(
            string countSheetId,
            string locationId,
            DateTime countDate,
            string requestedBy,
            string userId
        )
            : base(countSheetId, userId)
        {
            LocationId = locationId;
            CountDate = countDate;
            RequestedBy = requestedBy;
        }
    }

    public class CountSheetCategoryAddedEvent : CountSheetEvent
    {
        public string Category { get; }

        public CountSheetCategoryAddedEvent(string countSheetId, string category, string userId)
            : base(countSheetId, userId)
        {
            Category = category;
        }
    }

    public class CountSheetItemAddedEvent : CountSheetEvent
    {
        public string CountSheetItemId { get; }
        public string ItemId { get; }
        public string ItemName { get; }
        public string SKU { get; }
        public string Category { get; }
        public string UnitOfMeasure { get; }
        public double SystemQuantity { get; }

        public CountSheetItemAddedEvent(
            string countSheetId,
            string countSheetItemId,
            string itemId,
            string itemName,
            string sku,
            string category,
            string unitOfMeasure,
            double systemQuantity,
            string userId
        )
            : base(countSheetId, userId)
        {
            CountSheetItemId = countSheetItemId;
            ItemId = itemId;
            ItemName = itemName;
            SKU = sku;
            Category = category;
            UnitOfMeasure = unitOfMeasure;
            SystemQuantity = systemQuantity;
        }
    }

    public class CountSheetNotesSetEvent : CountSheetEvent
    {
        public string Notes { get; }

        public CountSheetNotesSetEvent(string countSheetId, string notes, string userId)
            : base(countSheetId, userId)
        {
            Notes = notes;
        }
    }

    public class CountSheetCountingStartedEvent : CountSheetEvent
    {
        public string CountedBy { get; }
        public DateTime StartTime { get; }

        public CountSheetCountingStartedEvent(
            string countSheetId,
            string countedBy,
            DateTime startTime,
            string userId
        )
            : base(countSheetId, userId)
        {
            CountedBy = countedBy;
            StartTime = startTime;
        }
    }

    public class CountSheetItemCountRecordedEvent : CountSheetEvent
    {
        public string ItemId { get; }
        public double SystemQuantity { get; }
        public double CountedQuantity { get; }
        public double Variance { get; }
        public double VariancePercentage { get; }
        public string BatchId { get; }

        public CountSheetItemCountRecordedEvent(
            string countSheetId,
            string itemId,
            double systemQuantity,
            double countedQuantity,
            double variance,
            double variancePercentage,
            string batchId,
            string userId
        )
            : base(countSheetId, userId)
        {
            ItemId = itemId;
            SystemQuantity = systemQuantity;
            CountedQuantity = countedQuantity;
            Variance = variance;
            VariancePercentage = variancePercentage;
            BatchId = batchId;
        }
    }

    public class CountSheetBatchCountRecordedEvent : CountSheetEvent
    {
        public string ItemId { get; }
        public string BatchId { get; }
        public string BatchNumber { get; }
        public DateTime ExpirationDate { get; }
        public double SystemQuantity { get; }
        public double CountedQuantity { get; }
        public double Variance { get; }

        public CountSheetBatchCountRecordedEvent(
            string countSheetId,
            string itemId,
            string batchId,
            string batchNumber,
            DateTime expirationDate,
            double systemQuantity,
            double countedQuantity,
            double variance,
            string userId
        )
            : base(countSheetId, userId)
        {
            ItemId = itemId;
            BatchId = batchId;
            BatchNumber = batchNumber;
            ExpirationDate = expirationDate;
            SystemQuantity = systemQuantity;
            CountedQuantity = countedQuantity;
            Variance = variance;
        }
    }

    public class CountSheetCompletedEvent : CountSheetEvent
    {
        public DateTime CompletedDate { get; }
        public int TotalItemsCounted { get; }
        public int ItemsWithVariance { get; }
        public decimal TotalVarianceValue { get; }

        public CountSheetCompletedEvent(
            string countSheetId,
            DateTime completedDate,
            int totalItemsCounted,
            int itemsWithVariance,
            decimal totalVarianceValue,
            string userId
        )
            : base(countSheetId, userId)
        {
            CompletedDate = completedDate;
            TotalItemsCounted = totalItemsCounted;
            ItemsWithVariance = itemsWithVariance;
            TotalVarianceValue = totalVarianceValue;
        }
    }

    public class CountSheetVarianceApprovedEvent : CountSheetEvent
    {
        public string ItemId { get; }
        public string ReasonCode { get; }
        public double Variance { get; }
        public decimal VarianceValue { get; }

        public CountSheetVarianceApprovedEvent(
            string countSheetId,
            string itemId,
            string reasonCode,
            double variance,
            decimal varianceValue,
            string userId
        )
            : base(countSheetId, userId)
        {
            ItemId = itemId;
            ReasonCode = reasonCode;
            Variance = variance;
            VarianceValue = varianceValue;
        }
    }

    public class CountSheetApprovedEvent : CountSheetEvent
    {
        public string ApprovedBy { get; }
        public DateTime ApprovalDate { get; }
        public int ItemsWithApprovedVariance { get; }
        public decimal TotalApprovedVarianceValue { get; }

        public CountSheetApprovedEvent(
            string countSheetId,
            string approvedBy,
            DateTime approvalDate,
            int itemsWithApprovedVariance,
            decimal totalApprovedVarianceValue,
            string userId
        )
            : base(countSheetId, userId)
        {
            ApprovedBy = approvedBy;
            ApprovalDate = approvalDate;
            ItemsWithApprovedVariance = itemsWithApprovedVariance;
            TotalApprovedVarianceValue = totalApprovedVarianceValue;
        }
    }
}
