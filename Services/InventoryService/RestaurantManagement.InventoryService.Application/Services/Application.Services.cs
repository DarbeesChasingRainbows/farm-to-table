using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Application.Commands.InventoryItems;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Queries.InventoryItems;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Application.Services
{
    /// <summary>
    /// Interface for inventory service
    /// </summary>
    public interface IInventoryService
    {
        // Inventory Items
        Task<string> CreateInventoryItemAsync(
            CreateInventoryItemCommand command,
            CancellationToken cancellationToken = default
        );
        Task<bool> UpdateInventoryItemAsync(
            UpdateInventoryItemCommand command,
            CancellationToken cancellationToken = default
        );
        Task<bool> DeleteInventoryItemAsync(
            string id,
            CancellationToken cancellationToken = default
        );
        Task<InventoryItemDto> GetInventoryItemByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        );
        Task<InventoryItemDto> GetInventoryItemBySKUAsync(
            string sku,
            CancellationToken cancellationToken = default
        );
        Task<(List<InventoryItemListDto> Items, int TotalCount)> GetInventoryItemsAsync(
            string? searchTerm,
            string? category,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default
        );
        Task<List<InventoryItemWithStockLevelDto>> GetInventoryItemsForReorderAsync(
            string? locationId,
            CancellationToken cancellationToken = default
        );
        Task<List<InventoryItemWithStockLevelDto>> GetInventoryItemsWithStockAtLocationAsync(
            string locationId,
            string? category,
            CancellationToken cancellationToken = default
        );
    }

    /// <summary>
    /// Implementation of inventory service
    /// </summary>
    public class InventoryService : IInventoryService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IMediator mediator, ILogger<InventoryService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // Inventory Items
        public async Task<string> CreateInventoryItemAsync(
            CreateInventoryItemCommand command,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Creating inventory item with SKU: {SKU}", command.SKU);
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<bool> UpdateInventoryItemAsync(
            UpdateInventoryItemCommand command,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Updating inventory item with ID: {InventoryItemId}",
                command.Id
            );
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<bool> DeleteInventoryItemAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Deleting inventory item with ID: {InventoryItemId}", id);
            var command = new DeleteInventoryItemCommand { Id = id };
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<InventoryItemDto> GetInventoryItemByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Getting inventory item by ID: {InventoryItemId}", id);
            var query = new GetInventoryItemByIdQuery { Id = id };
            return await _mediator.Send(query, cancellationToken);
        }

        public async Task<InventoryItemDto> GetInventoryItemBySKUAsync(
            string sku,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Getting inventory item by SKU: {SKU}", sku);
            var query = new GetInventoryItemBySKUQuery { SKU = sku };
            return await _mediator.Send(query, cancellationToken);
        }

        public async Task<(
            List<InventoryItemListDto> Items,
            int TotalCount
        )> GetInventoryItemsAsync(
            string? searchTerm,
            string? category,
            bool? isActive,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Getting inventory items with filters: SearchTerm={SearchTerm}, Category={Category}, IsActive={IsActive}, Page={Page}, PageSize={PageSize}",
                searchTerm,
                category,
                isActive,
                pageNumber,
                pageSize
            );

            var query = new GetInventoryItemsQuery
            {
                SearchTerm = searchTerm,
                Category = category,
                IsActive = isActive,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return await _mediator.Send(query, cancellationToken);
        }

        public async Task<List<InventoryItemWithStockLevelDto>> GetInventoryItemsForReorderAsync(
            string? locationId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Getting inventory items for reorder. LocationId={LocationId}",
                locationId
            );
            var query = new GetInventoryItemsForReorderQuery { LocationId = locationId };
            return await _mediator.Send(query, cancellationToken);
        }

        public async Task<
            List<InventoryItemWithStockLevelDto>
        > GetInventoryItemsWithStockAtLocationAsync(
            string locationId,
            string? category,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Getting inventory items with stock at location. LocationId={LocationId}, Category={Category}",
                locationId,
                category
            );
            var query = new GetInventoryItemsWithStockAtLocationQuery
            {
                LocationId = locationId,
                Category = category
            };
            return await _mediator.Send(query, cancellationToken);
        }
    }

    /// <summary>
    /// Interface for transaction service
    /// </summary>
    public interface ITransactionService
    {
        Task<string> CreateTransactionAsync(
            CreateInventoryTransactionCommand command,
            CancellationToken cancellationToken = default
        );
        Task<bool> CompleteTransactionAsync(
            string transactionId,
            CancellationToken cancellationToken = default
        );
        Task<bool> CancelTransactionAsync(
            string transactionId,
            string reason,
            CancellationToken cancellationToken = default
        );
        Task<InventoryTransactionDto> GetTransactionByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        );
        Task<List<InventoryTransactionDto>> GetPendingTransactionsAsync(
            CancellationToken cancellationToken = default
        );
        Task<List<InventoryTransactionDto>> GetTransactionsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            string? transactionType,
            CancellationToken cancellationToken = default
        );
    }

    /// <summary>
    /// Implementation of transaction service
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(IMediator mediator, ILogger<TransactionService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<string> CreateTransactionAsync(
            CreateInventoryTransactionCommand command,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Creating inventory transaction of type: {TransactionType}",
                command.TransactionType
            );
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<bool> CompleteTransactionAsync(
            string transactionId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Completing inventory transaction with ID: {TransactionId}",
                transactionId
            );
            var command = new CompleteInventoryTransactionCommand { TransactionId = transactionId };
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<bool> CancelTransactionAsync(
            string transactionId,
            string reason,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Cancelling inventory transaction with ID: {TransactionId}. Reason: {Reason}",
                transactionId,
                reason
            );
            var command = new CancelInventoryTransactionCommand
            {
                TransactionId = transactionId,
                Reason = reason
            };
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<InventoryTransactionDto> GetTransactionByIdAsync(
            string id,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Getting inventory transaction by ID: {TransactionId}", id);
            var query = new GetInventoryTransactionByIdQuery { Id = id };
            return await _mediator.Send(query, cancellationToken);
        }

        public async Task<List<InventoryTransactionDto>> GetPendingTransactionsAsync(
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation("Getting pending inventory transactions");
            var query = new GetPendingInventoryTransactionsQuery();
            return await _mediator.Send(query, cancellationToken);
        }

        public async Task<List<InventoryTransactionDto>> GetTransactionsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            string? transactionType,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Getting inventory transactions in date range: {StartDate} to {EndDate}. Type: {TransactionType}",
                startDate,
                endDate,
                transactionType
            );

            var query = new GetInventoryTransactionsByDateRangeQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                TransactionType = transactionType
            };

            return await _mediator.Send(query, cancellationToken);
        }
    }

    /// <summary>
    /// Interface for reporting service
    /// </summary>
    public interface IReportingService
    {
        Task<byte[]> GenerateInventoryValueReportAsync(
            string? locationId,
            string? category,
            CancellationToken cancellationToken = default
        );
        Task<byte[]> GenerateInventoryMovementReportAsync(
            DateTime startDate,
            DateTime endDate,
            string? inventoryItemId,
            string? locationId,
            CancellationToken cancellationToken = default
        );
        Task<byte[]> GenerateExpiringItemsReportAsync(
            DateTime cutoffDate,
            string? locationId,
            CancellationToken cancellationToken = default
        );
        Task<byte[]> GenerateInventoryCountVarianceReportAsync(
            string countSheetId,
            CancellationToken cancellationToken = default
        );
    }

    /// <summary>
    /// Implementation of reporting service
    /// </summary>
    public class ReportingService : IReportingService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReportingService> _logger;

        public ReportingService(IMediator mediator, ILogger<ReportingService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<byte[]> GenerateInventoryValueReportAsync(
            string? locationId,
            string? category,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Generating inventory value report. LocationId={LocationId}, Category={Category}",
                locationId,
                category
            );
            var command = new GenerateInventoryValueReportCommand
            {
                LocationId = locationId,
                Category = category
            };
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<byte[]> GenerateInventoryMovementReportAsync(
            DateTime startDate,
            DateTime endDate,
            string? inventoryItemId,
            string? locationId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Generating inventory movement report. StartDate={StartDate}, EndDate={EndDate}, InventoryItemId={InventoryItemId}, LocationId={LocationId}",
                startDate,
                endDate,
                inventoryItemId,
                locationId
            );

            var command = new GenerateInventoryMovementReportCommand
            {
                StartDate = startDate,
                EndDate = endDate,
                InventoryItemId = inventoryItemId,
                LocationId = locationId
            };

            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<byte[]> GenerateExpiringItemsReportAsync(
            DateTime cutoffDate,
            string? locationId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Generating expiring items report. CutoffDate={CutoffDate}, LocationId={LocationId}",
                cutoffDate,
                locationId
            );
            var command = new GenerateExpiringItemsReportCommand
            {
                CutoffDate = cutoffDate,
                LocationId = locationId
            };
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<byte[]> GenerateInventoryCountVarianceReportAsync(
            string countSheetId,
            CancellationToken cancellationToken = default
        )
        {
            _logger.LogInformation(
                "Generating inventory count variance report. CountSheetId={CountSheetId}",
                countSheetId
            );
            var command = new GenerateInventoryCountVarianceReportCommand
            {
                CountSheetId = countSheetId
            };
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
