using FluentValidation;
using RestaurantManagement.InventoryService.Application.Commands.InventoryItems;

namespace RestaurantManagement.InventoryService.Application.Validators
{
    /// <summary>
    /// Validator for inventory transaction inputs
    /// </summary>
    public class CreateInventoryTransactionValidator
        : AbstractValidator<CreateInventoryTransactionCommand>
    {
        public CreateInventoryTransactionValidator()
        {
            RuleFor(x => x.TransactionType)
                .NotEmpty()
                .WithMessage("Transaction type is required.")
                .Must(BeValidTransactionType)
                .WithMessage(
                    "Invalid transaction type. Allowed values: Receipt, Consumption, Transfer, Adjustment."
                );

            When(
                x => x.TransactionType == "Receipt",
                () =>
                {
                    RuleFor(x => x.DestinationLocationId)
                        .NotEmpty()
                        .WithMessage("Destination location is required for receipt transactions.");

                    RuleFor(x => x.VendorId)
                        .NotEmpty()
                        .WithMessage("Vendor is required for receipt transactions.");
                }
            );

            When(
                x => x.TransactionType == "Consumption",
                () =>
                {
                    RuleFor(x => x.SourceLocationId)
                        .NotEmpty()
                        .WithMessage("Source location is required for consumption transactions.");
                }
            );

            When(
                x => x.TransactionType == "Transfer",
                () =>
                {
                    RuleFor(x => x.SourceLocationId)
                        .NotEmpty()
                        .WithMessage("Source location is required for transfer transactions.");

                    RuleFor(x => x.DestinationLocationId)
                        .NotEmpty()
                        .WithMessage("Destination location is required for transfer transactions.")
                        .NotEqual(x => x.SourceLocationId)
                        .WithMessage("Source and destination locations must be different.");
                }
            );

            When(
                x => x.TransactionType == "Adjustment",
                () =>
                {
                    RuleFor(x => x.DestinationLocationId)
                        .NotEmpty()
                        .WithMessage("Location is required for adjustment transactions.");

                    RuleFor(x => x.Notes)
                        .NotEmpty()
                        .WithMessage(
                            "Notes are required for adjustment transactions to explain the reason for the adjustment."
                        );
                }
            );

            RuleFor(x => x.TransactionDate)
                .NotEmpty()
                .WithMessage("Transaction date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Transaction date cannot be in the future.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required.")
                .ForEach(item =>
                {
                    item.ChildRules(itemRules =>
                    {
                        itemRules
                            .RuleFor(i => i.InventoryItemId)
                            .NotEmpty()
                            .WithMessage("Inventory item ID is required.");

                        itemRules
                            .RuleFor(i => i.Quantity)
                            .GreaterThan(0)
                            .WithMessage("Quantity must be greater than zero.");

                        itemRules.When(
                            i => x.TransactionType == "Receipt",
                            () =>
                            {
                                itemRules
                                    .RuleFor(i => i.UnitPrice)
                                    .NotNull()
                                    .WithMessage("Unit price is required for receipt transactions.")
                                    .GreaterThan(0)
                                    .WithMessage("Unit price must be greater than zero.");

                                itemRules
                                    .RuleFor(i => i.BatchId)
                                    .NotEmpty()
                                    .WithMessage("Batch ID is required for receipt transactions.");
                            }
                        );
                    });
                });
        }

        private bool BeValidTransactionType(string transactionType)
        {
            var validTypes = new[] { "Receipt", "Consumption", "Transfer", "Adjustment" };
            return validTypes.Contains(transactionType);
        }
    }

    /// <summary>
    /// Validator for count sheet inputs
    /// </summary>
    public class CreateCountSheetValidator : AbstractValidator<CreateCountSheetCommand>
    {
        public CreateCountSheetValidator()
        {
            RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location ID is required.");

            RuleFor(x => x.CountDate)
                .NotEmpty()
                .WithMessage("Count date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Count date cannot be in the future.");

            // Optional: Allow only certain categories to be included
            RuleFor(x => x.IncludeCategories)
                .ForEach(category =>
                {
                    category.Must(BeValidCategory).WithMessage("Invalid category.");
                });
        }

        private bool BeValidCategory(string category)
        {
            // Define your valid categories here or fetch from a repository
            var validCategories = new[]
            {
                "Produce",
                "Meat",
                "Seafood",
                "Dairy",
                "Dry Goods",
                "Beverages",
                "Spices",
                "Cleaning Supplies",
                "Paper Goods",
                "All"
            };

            return string.IsNullOrEmpty(category) || validCategories.Contains(category);
        }
    }

    /// <summary>
    /// Validator for completing count sheet inputs
    /// </summary>
    public class CompleteCountSheetValidator : AbstractValidator<CompleteCountSheetCommand>
    {
        public CompleteCountSheetValidator()
        {
            RuleFor(x => x.CountSheetId).NotEmpty().WithMessage("Count sheet ID is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one item is required.")
                .ForEach(item =>
                {
                    item.ChildRules(itemRules =>
                    {
                        itemRules
                            .RuleFor(i => i.InventoryItemId)
                            .NotEmpty()
                            .WithMessage("Inventory item ID is required.");

                        itemRules
                            .RuleFor(i => i.ActualQuantity)
                            .NotNull()
                            .WithMessage("Actual quantity is required.")
                            .GreaterThanOrEqualTo(0)
                            .WithMessage("Actual quantity must be greater than or equal to zero.");

                        // Variance rules are typically calculated, not input directly
                    });
                });
        }
    }

    /// <summary>
    /// Validator for creating batch inputs
    /// </summary>
    public class CreateBatchValidator : AbstractValidator<CreateBatchCommand>
    {
        public CreateBatchValidator()
        {
            RuleFor(x => x.InventoryItemId)
                .NotEmpty()
                .WithMessage("Inventory item ID is required.");

            RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location ID is required.");

            RuleFor(x => x.BatchNumber)
                .NotEmpty()
                .WithMessage("Batch number is required.")
                .MaximumLength(50)
                .WithMessage("Batch number must not exceed 50 characters.");

            RuleFor(x => x.ExpirationDate)
                .NotEmpty()
                .WithMessage("Expiration date is required.")
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Expiration date must be in the future.");

            RuleFor(x => x.InitialQuantity)
                .GreaterThan(0)
                .WithMessage("Initial quantity must be greater than zero.");
        }
    }

    /// <summary>
    /// Validator for vendor inputs
    /// </summary>
    public class CreateVendorValidator : AbstractValidator<CreateVendorCommand>
    {
        public CreateVendorValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Vendor name is required.")
                .MaximumLength(100)
                .WithMessage("Vendor name must not exceed 100 characters.");

            RuleFor(x => x.ContactName)
                .NotEmpty()
                .WithMessage("Contact name is required.")
                .MaximumLength(100)
                .WithMessage("Contact name must not exceed 100 characters.");

            RuleFor(x => x.ContactEmail)
                .NotEmpty()
                .WithMessage("Contact email is required.")
                .EmailAddress()
                .WithMessage("Invalid email address format.")
                .MaximumLength(100)
                .WithMessage("Contact email must not exceed 100 characters.");

            RuleFor(x => x.ContactPhone)
                .NotEmpty()
                .WithMessage("Contact phone is required.")
                .Matches(@"^\+?[0-9\s\-\(\)]+$")
                .WithMessage("Invalid phone number format.")
                .MaximumLength(20)
                .WithMessage("Contact phone must not exceed 20 characters.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(255)
                .WithMessage("Address must not exceed 255 characters.");
        }
    }

    /// <summary>
    /// Validator for creating location inputs
    /// </summary>
    public class CreateLocationValidator : AbstractValidator<CreateLocationCommand>
    {
        public CreateLocationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Location name is required.")
                .MaximumLength(100)
                .WithMessage("Location name must not exceed 100 characters.");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Location type is required.")
                .Must(BeValidLocationType)
                .WithMessage(
                    "Invalid location type. Allowed values: Restaurant, Warehouse, Prep Kitchen, Bar, Storage."
                );

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(255)
                .WithMessage("Address must not exceed 255 characters.");
        }

        private bool BeValidLocationType(string locationType)
        {
            var validTypes = new[] { "Restaurant", "Warehouse", "Prep Kitchen", "Bar", "Storage" };
            return validTypes.Contains(locationType);
        }
    }

    /// <summary>
    /// Validator for reservation inputs
    /// </summary>
    public class CreateReservationValidator : AbstractValidator<CreateReservationCommand>
    {
        public CreateReservationValidator()
        {
            RuleFor(x => x.InventoryItemId)
                .NotEmpty()
                .WithMessage("Inventory item ID is required.");

            RuleFor(x => x.LocationId).NotEmpty().WithMessage("Location ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.ReservationDate).NotEmpty().WithMessage("Reservation date is required.");

            RuleFor(x => x.ExpirationDate)
                .NotEmpty()
                .WithMessage("Expiration date is required.")
                .GreaterThan(x => x.ReservationDate)
                .WithMessage("Expiration date must be after reservation date.");

            RuleFor(x => x.Purpose)
                .NotEmpty()
                .WithMessage("Purpose is required.")
                .MaximumLength(255)
                .WithMessage("Purpose must not exceed 255 characters.");
        }
    }
}
