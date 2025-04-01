using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Events;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Application.Commands.InventoryItems
{
    #region CreateInventoryItem

    /// <summary>
    /// Command to create a new inventory item
    /// </summary>
    public class CreateInventoryItemCommand : IRequest<string>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public double ReorderThreshold { get; set; }
        public double MinimumOrderQuantity { get; set; }
        public double DefaultOrderQuantity { get; set; }
    }

    /// <summary>
    /// Validator for CreateInventoryItemCommand
    /// </summary>
    public class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
    {
        public CreateInventoryItemCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(v => v.SKU)
                .NotEmpty()
                .WithMessage("SKU is required.")
                .MaximumLength(50)
                .WithMessage("SKU must not exceed 50 characters.");

            RuleFor(v => v.Category)
                .NotEmpty()
                .WithMessage("Category is required.")
                .MaximumLength(50)
                .WithMessage("Category must not exceed 50 characters.");

            RuleFor(v => v.UnitOfMeasure)
                .NotEmpty()
                .WithMessage("Unit of measure is required.")
                .MaximumLength(20)
                .WithMessage("Unit of measure must not exceed 20 characters.");

            RuleFor(v => v.ReorderThreshold)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Reorder threshold must be non-negative.");

            RuleFor(v => v.MinimumOrderQuantity)
                .GreaterThan(0)
                .WithMessage("Minimum order quantity must be greater than zero.");

            RuleFor(v => v.DefaultOrderQuantity)
                .GreaterThan(0)
                .WithMessage("Default order quantity must be greater than zero.")
                .GreaterThanOrEqualTo(v => v.MinimumOrderQuantity)
                .WithMessage(
                    "Default order quantity must be greater than or equal to minimum order quantity."
                );
        }
    }

    /// <summary>
    /// Handler for CreateInventoryItemCommand
    /// </summary>
    public class CreateInventoryItemCommandHandler
        : IRequestHandler<CreateInventoryItemCommand, string>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CreateInventoryItemCommandHandler> _logger;

        public CreateInventoryItemCommandHandler(
            IInventoryItemRepository inventoryItemRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<CreateInventoryItemCommandHandler> logger
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<string> Handle(
            CreateInventoryItemCommand request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation("Creating new inventory item with SKU: {SKU}", request.SKU);

            // Check if item with the same SKU already exists
            var existingItem = await _inventoryItemRepository.FindBySKUAsync(
                request.SKU,
                cancellationToken
            );
            if (existingItem != null)
            {
                throw new Application.Exceptions.BusinessRuleViolationException(
                    $"Inventory item with SKU '{request.SKU}' already exists."
                );
            }

            // Create a new inventory item
            var inventoryItem = new InventoryItem
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                SKU = request.SKU,
                Category = request.Category,
                UnitOfMeasure = request.UnitOfMeasure,
                ReorderThreshold = request.ReorderThreshold,
                MinimumOrderQuantity = request.MinimumOrderQuantity,
                DefaultOrderQuantity = request.DefaultOrderQuantity,
                IsActive = true,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            // Add item to repository
            await _inventoryItemRepository.AddAsync(inventoryItem, cancellationToken);

            // Commit changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Inventory item created with ID: {InventoryItemId}",
                inventoryItem.Id
            );

            return inventoryItem.Id;
        }
    }

    #endregion

    #region UpdateInventoryItem

    /// <summary>
    /// Command to update an existing inventory item
    /// </summary>
    public class UpdateInventoryItemCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public double ReorderThreshold { get; set; }
        public double MinimumOrderQuantity { get; set; }
        public double DefaultOrderQuantity { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Validator for UpdateInventoryItemCommand
    /// </summary>
    public class UpdateInventoryItemCommandValidator : AbstractValidator<UpdateInventoryItemCommand>
    {
        public UpdateInventoryItemCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("ID is required.");

            RuleFor(v => v.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(v => v.Category)
                .NotEmpty()
                .WithMessage("Category is required.")
                .MaximumLength(50)
                .WithMessage("Category must not exceed 50 characters.");

            RuleFor(v => v.UnitOfMeasure)
                .NotEmpty()
                .WithMessage("Unit of measure is required.")
                .MaximumLength(20)
                .WithMessage("Unit of measure must not exceed 20 characters.");

            RuleFor(v => v.ReorderThreshold)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Reorder threshold must be non-negative.");

            RuleFor(v => v.MinimumOrderQuantity)
                .GreaterThan(0)
                .WithMessage("Minimum order quantity must be greater than zero.");

            RuleFor(v => v.DefaultOrderQuantity)
                .GreaterThan(0)
                .WithMessage("Default order quantity must be greater than zero.")
                .GreaterThanOrEqualTo(v => v.MinimumOrderQuantity)
                .WithMessage(
                    "Default order quantity must be greater than or equal to minimum order quantity."
                );
        }
    }

    /// <summary>
    /// Handler for UpdateInventoryItemCommand
    /// </summary>
    public class UpdateInventoryItemCommandHandler
        : IRequestHandler<UpdateInventoryItemCommand, bool>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateInventoryItemCommandHandler> _logger;

        public UpdateInventoryItemCommandHandler(
            IInventoryItemRepository inventoryItemRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<UpdateInventoryItemCommandHandler> logger
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<bool> Handle(
            UpdateInventoryItemCommand request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Updating inventory item with ID: {InventoryItemId}",
                request.Id
            );

            // Get the existing inventory item
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                request.Id,
                cancellationToken
            );
            if (inventoryItem == null)
            {
                throw new Application.Exceptions.NotFoundException(
                    nameof(InventoryItem),
                    request.Id
                );
            }

            // Update properties
            inventoryItem.Name = request.Name;
            inventoryItem.Description = request.Description;
            inventoryItem.Category = request.Category;
            inventoryItem.UnitOfMeasure = request.UnitOfMeasure;
            inventoryItem.ReorderThreshold = request.ReorderThreshold;
            inventoryItem.MinimumOrderQuantity = request.MinimumOrderQuantity;
            inventoryItem.DefaultOrderQuantity = request.DefaultOrderQuantity;
            inventoryItem.IsActive = request.IsActive;
            inventoryItem.LastModifiedBy = _currentUserService.UserId;
            inventoryItem.LastModifiedAt = DateTime.UtcNow;

            // Update in repository
            _inventoryItemRepository.Update(inventoryItem);

            // Commit changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Inventory item updated: {InventoryItemId}", inventoryItem.Id);

            return true;
        }
    }

    #endregion

    #region DeleteInventoryItem

    /// <summary>
    /// Command to delete an inventory item
    /// </summary>
    public class DeleteInventoryItemCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Validator for DeleteInventoryItemCommand
    /// </summary>
    public class DeleteInventoryItemCommandValidator : AbstractValidator<DeleteInventoryItemCommand>
    {
        public DeleteInventoryItemCommandValidator()
        {
            RuleFor(v => v.Id).NotEmpty().WithMessage("ID is required.");
        }
    }

    /// <summary>
    /// Handler for DeleteInventoryItemCommand
    /// </summary>
    public class DeleteInventoryItemCommandHandler
        : IRequestHandler<DeleteInventoryItemCommand, bool>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IStockLevelRepository _stockLevelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<DeleteInventoryItemCommandHandler> _logger;

        public DeleteInventoryItemCommandHandler(
            IInventoryItemRepository inventoryItemRepository,
            IStockLevelRepository stockLevelRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<DeleteInventoryItemCommandHandler> logger
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _stockLevelRepository = stockLevelRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<bool> Handle(
            DeleteInventoryItemCommand request,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Deleting inventory item with ID: {InventoryItemId}",
                request.Id
            );

            // Get the existing inventory item
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                request.Id,
                cancellationToken
            );
            if (inventoryItem == null)
            {
                throw new Application.Exceptions.NotFoundException(
                    nameof(InventoryItem),
                    request.Id
                );
            }

            // Check if there are stock levels associated with this item
            var stockLevels = await _stockLevelRepository.GetByInventoryItemIdAsync(
                request.Id,
                cancellationToken
            );
            if (stockLevels.Any())
            {
                throw new Application.Exceptions.BusinessRuleViolationException(
                    "Cannot delete inventory item that has associated stock levels. Deactivate it instead."
                );
            }

            // Delete from repository
            _inventoryItemRepository.Delete(inventoryItem);

            // Commit changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Inventory item deleted: {InventoryItemId}", inventoryItem.Id);

            return true;
        }
    }

    #endregion
}
