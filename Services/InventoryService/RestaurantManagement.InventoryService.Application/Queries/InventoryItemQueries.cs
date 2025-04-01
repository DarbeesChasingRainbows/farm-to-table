using AutoMapper;
using FluentValidation;
using MediatR;
using RestaurantManagement.InventoryService.Application.Exceptions;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Application.Queries.InventoryItems
{
    #region DTOs

    /// <summary>
    /// DTO for inventory item details
    /// </summary>
    public class InventoryItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public double ReorderThreshold { get; set; }
        public double MinimumOrderQuantity { get; set; }
        public double DefaultOrderQuantity { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    /// <summary>
    /// DTO for inventory item list
    /// </summary>
    public class InventoryItemListDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO for inventory item with stock level
    /// </summary>
    public class InventoryItemWithStockLevelDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public double CurrentQuantity { get; set; }
        public double ReorderThreshold { get; set; }
        public double MinimumOrderQuantity { get; set; }
        public double DefaultOrderQuantity { get; set; }
        public bool IsActive { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
    }

    #endregion

    #region GetInventoryItemById

    /// <summary>
    /// Query to get an inventory item by ID
    /// </summary>
    public class GetInventoryItemByIdQuery : IRequest<InventoryItemDto>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validator for GetInventoryItemByIdQuery
    /// </summary>
    public class GetInventoryItemByIdQueryValidator : AbstractValidator<GetInventoryItemByIdQuery>
    {
        public GetInventoryItemByIdQueryValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("ID is required.");
        }
    }

    /// <summary>
    /// Handler for GetInventoryItemByIdQuery
    /// </summary>
    public class GetInventoryItemByIdQueryHandler
        : IRequestHandler<GetInventoryItemByIdQuery, InventoryItemDto>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IMapper _mapper;

        public GetInventoryItemByIdQueryHandler(
            IInventoryItemRepository inventoryItemRepository,
            IMapper mapper
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _mapper = mapper;
        }

        public async Task<InventoryItemDto> Handle(
            GetInventoryItemByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                request.Id,
                cancellationToken
            );

            if (inventoryItem == null)
            {
                throw new NotFoundException(nameof(InventoryItem), request.Id);
            }

            return _mapper.Map<InventoryItemDto>(inventoryItem);
        }
    }

    #endregion

    #region GetInventoryItemBySKU

    /// <summary>
    /// Query to get an inventory item by SKU
    /// </summary>
    public class GetInventoryItemBySKUQuery : IRequest<InventoryItemDto>
    {
        public string SKU { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validator for GetInventoryItemBySKUQuery
    /// </summary>
    public class GetInventoryItemBySKUQueryValidator : AbstractValidator<GetInventoryItemBySKUQuery>
    {
        public GetInventoryItemBySKUQueryValidator()
        {
            RuleFor(v => v.SKU).NotEmpty().WithMessage("SKU is required.");
        }
    }

    /// <summary>
    /// Handler for GetInventoryItemBySKUQuery
    /// </summary>
    public class GetInventoryItemBySKUQueryHandler
        : IRequestHandler<GetInventoryItemBySKUQuery, InventoryItemDto>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IMapper _mapper;

        public GetInventoryItemBySKUQueryHandler(
            IInventoryItemRepository inventoryItemRepository,
            IMapper mapper
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _mapper = mapper;
        }

        public async Task<InventoryItemDto> Handle(
            GetInventoryItemBySKUQuery request,
            CancellationToken cancellationToken
        )
        {
            var inventoryItem = await _inventoryItemRepository.FindBySKUAsync(
                request.SKU,
                cancellationToken
            );

            if (inventoryItem == null)
            {
                throw new NotFoundException(nameof(InventoryItem), request.SKU);
            }

            return _mapper.Map<InventoryItemDto>(inventoryItem);
        }
    }

    #endregion

    #region GetInventoryItems

    /// <summary>
    /// Query to get a list of inventory items with filtering and pagination
    /// </summary>
    public class GetInventoryItemsQuery
        : IRequest<(List<InventoryItemListDto> Items, int TotalCount)>
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Validator for GetInventoryItemsQuery
    /// </summary>
    public class GetInventoryItemsQueryValidator : AbstractValidator<GetInventoryItemsQuery>
    {
        public GetInventoryItemsQueryValidator()
        {
            RuleFor(v => v.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(v => v.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size must not exceed 100.");
        }
    }

    /// <summary>
    /// Handler for GetInventoryItemsQuery
    /// </summary>
    public class GetInventoryItemsQueryHandler
        : IRequestHandler<
            GetInventoryItemsQuery,
            (List<InventoryItemListDto> Items, int TotalCount)
        >
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IMapper _mapper;

        public GetInventoryItemsQueryHandler(
            IInventoryItemRepository inventoryItemRepository,
            IMapper mapper
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _mapper = mapper;
        }

        public async Task<(List<InventoryItemListDto> Items, int TotalCount)> Handle(
            GetInventoryItemsQuery request,
            CancellationToken cancellationToken
        )
        {
            var (items, totalCount) = await _inventoryItemRepository.GetInventoryItemsAsync(
                request.SearchTerm,
                request.Category,
                request.IsActive,
                request.PageNumber,
                request.PageSize,
                cancellationToken
            );

            var itemDtos = _mapper.Map<List<InventoryItemListDto>>(items);

            return (itemDtos, totalCount);
        }
    }

    #endregion

    #region GetInventoryItemsForReorder

    /// <summary>
    /// Query to get inventory items that need to be reordered
    /// </summary>
    public class GetInventoryItemsForReorderQuery : IRequest<List<InventoryItemWithStockLevelDto>>
    {
        public string? LocationId { get; set; }
    }

    /// <summary>
    /// Handler for GetInventoryItemsForReorderQuery
    /// </summary>
    public class GetInventoryItemsForReorderQueryHandler
        : IRequestHandler<GetInventoryItemsForReorderQuery, List<InventoryItemWithStockLevelDto>>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IStockLevelRepository _stockLevelRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public GetInventoryItemsForReorderQueryHandler(
            IInventoryItemRepository inventoryItemRepository,
            IStockLevelRepository stockLevelRepository,
            ILocationRepository locationRepository,
            IMapper mapper
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _stockLevelRepository = stockLevelRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<List<InventoryItemWithStockLevelDto>> Handle(
            GetInventoryItemsForReorderQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = new List<InventoryItemWithStockLevelDto>();

            // If a specific location is requested
            if (!string.IsNullOrEmpty(request.LocationId))
            {
                var location = await _locationRepository.GetByIdAsync(
                    request.LocationId,
                    cancellationToken
                );
                if (location == null)
                {
                    throw new NotFoundException(nameof(Location), request.LocationId);
                }

                var stockLevels = await _stockLevelRepository.GetLowStockItemsAsync(
                    request.LocationId,
                    cancellationToken
                );

                foreach (var stockLevel in stockLevels)
                {
                    var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                        stockLevel.InventoryItemId,
                        cancellationToken
                    );
                    if (inventoryItem != null && inventoryItem.IsActive)
                    {
                        result.Add(
                            new InventoryItemWithStockLevelDto
                            {
                                Id = inventoryItem.Id,
                                Name = inventoryItem.Name,
                                SKU = inventoryItem.SKU,
                                Category = inventoryItem.Category,
                                UnitOfMeasure = inventoryItem.UnitOfMeasure,
                                CurrentQuantity = stockLevel.CurrentQuantity,
                                ReorderThreshold = inventoryItem.ReorderThreshold,
                                MinimumOrderQuantity = inventoryItem.MinimumOrderQuantity,
                                DefaultOrderQuantity = inventoryItem.DefaultOrderQuantity,
                                IsActive = inventoryItem.IsActive,
                                LocationId = location.Id,
                                LocationName = location.Name
                            }
                        );
                    }
                }
            }
            // Get items that need reordering across all locations
            else
            {
                var locations = await _locationRepository.GetAllAsync(cancellationToken);

                foreach (var location in locations.Where(l => l.IsActive))
                {
                    var stockLevels = await _stockLevelRepository.GetLowStockItemsAsync(
                        location.Id,
                        cancellationToken
                    );

                    foreach (var stockLevel in stockLevels)
                    {
                        var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                            stockLevel.InventoryItemId,
                            cancellationToken
                        );
                        if (inventoryItem != null && inventoryItem.IsActive)
                        {
                            result.Add(
                                new InventoryItemWithStockLevelDto
                                {
                                    Id = inventoryItem.Id,
                                    Name = inventoryItem.Name,
                                    SKU = inventoryItem.SKU,
                                    Category = inventoryItem.Category,
                                    UnitOfMeasure = inventoryItem.UnitOfMeasure,
                                    CurrentQuantity = stockLevel.CurrentQuantity,
                                    ReorderThreshold = inventoryItem.ReorderThreshold,
                                    MinimumOrderQuantity = inventoryItem.MinimumOrderQuantity,
                                    DefaultOrderQuantity = inventoryItem.DefaultOrderQuantity,
                                    IsActive = inventoryItem.IsActive,
                                    LocationId = location.Id,
                                    LocationName = location.Name
                                }
                            );
                        }
                    }
                }
            }

            return result;
        }
    }

    #endregion

    #region GetInventoryItemsWithStockAtLocation

    /// <summary>
    /// Query to get inventory items with stock levels at a specific location
    /// </summary>
    public class GetInventoryItemsWithStockAtLocationQuery
        : IRequest<List<InventoryItemWithStockLevelDto>>
    {
        public string LocationId { get; set; } = string.Empty;
        public string? Category { get; set; }
    }

    /// <summary>
    /// Validator for GetInventoryItemsWithStockAtLocationQuery
    /// </summary>
    public class GetInventoryItemsWithStockAtLocationQueryValidator
        : AbstractValidator<GetInventoryItemsWithStockAtLocationQuery>
    {
        public GetInventoryItemsWithStockAtLocationQueryValidator()
        {
            RuleFor(v => v.LocationId).NotEmpty().WithMessage("Location ID is required.");
        }
    }

    /// <summary>
    /// Handler for GetInventoryItemsWithStockAtLocationQuery
    /// </summary>
    public class GetInventoryItemsWithStockAtLocationQueryHandler
        : IRequestHandler<
            GetInventoryItemsWithStockAtLocationQuery,
            List<InventoryItemWithStockLevelDto>
        >
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IStockLevelRepository _stockLevelRepository;
        private readonly ILocationRepository _locationRepository;

        public GetInventoryItemsWithStockAtLocationQueryHandler(
            IInventoryItemRepository inventoryItemRepository,
            IStockLevelRepository stockLevelRepository,
            ILocationRepository locationRepository
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _stockLevelRepository = stockLevelRepository;
            _locationRepository = locationRepository;
        }

        public async Task<List<InventoryItemWithStockLevelDto>> Handle(
            GetInventoryItemsWithStockAtLocationQuery request,
            CancellationToken cancellationToken
        )
        {
            var result = new List<InventoryItemWithStockLevelDto>();

            // Verify the location exists
            var location = await _locationRepository.GetByIdAsync(
                request.LocationId,
                cancellationToken
            );
            if (location == null)
            {
                throw new NotFoundException(nameof(Location), request.LocationId);
            }

            // Get all inventory items, filtered by category if provided
            var inventoryItems = await _inventoryItemRepository.GetAllAsync(cancellationToken);
            if (!string.IsNullOrEmpty(request.Category))
            {
                inventoryItems = inventoryItems.Where(i => i.Category == request.Category).ToList();
            }

            // Get all stock levels for the location
            var stockLevels = await _stockLevelRepository.GetByLocationIdAsync(
                request.LocationId,
                cancellationToken
            );

            // Create a lookup dictionary for quick access
            var stockLevelDict = stockLevels.ToDictionary(sl => sl.InventoryItemId);

            foreach (var item in inventoryItems.Where(i => i.IsActive))
            {
                // Check if there's a stock level for this item at the location
                if (stockLevelDict.TryGetValue(item.Id, out var stockLevel))
                {
                    result.Add(
                        new InventoryItemWithStockLevelDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SKU = item.SKU,
                            Category = item.Category,
                            UnitOfMeasure = item.UnitOfMeasure,
                            CurrentQuantity = stockLevel.CurrentQuantity,
                            ReorderThreshold = item.ReorderThreshold,
                            MinimumOrderQuantity = item.MinimumOrderQuantity,
                            DefaultOrderQuantity = item.DefaultOrderQuantity,
                            IsActive = item.IsActive,
                            LocationId = location.Id,
                            LocationName = location.Name
                        }
                    );
                }
                else
                {
                    // Item exists but has no stock at this location
                    result.Add(
                        new InventoryItemWithStockLevelDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            SKU = item.SKU,
                            Category = item.Category,
                            UnitOfMeasure = item.UnitOfMeasure,
                            CurrentQuantity = 0, // No stock
                            ReorderThreshold = item.ReorderThreshold,
                            MinimumOrderQuantity = item.MinimumOrderQuantity,
                            DefaultOrderQuantity = item.DefaultOrderQuantity,
                            IsActive = item.IsActive,
                            LocationId = location.Id,
                            LocationName = location.Name
                        }
                    );
                }
            }

            return result;
        }
    }

    #endregion
}
