using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

/// <summary>
/// Represents a form for adding new product with comprehensive validation rules.
/// Inherits from ValidatorBase to provide validation functionality.
/// </summary>
public class AddProductForm : ValidatorBase
{
    #region Public Properties
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// Optional property that can be null.
    /// </summary>
    public int? ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// Validated for non-empty and minimum length (2 chars).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the category ID of the product.
    /// Validated for non-empty value.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// Validated for non-empty and minimum value (0).
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement for the product.
    /// Validated for non-empty and minimum length (1 char).
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// Gets or sets the discounted price of the product.
    /// Optional property validated for minimum value (0).
    /// </summary>
    public decimal? DiscountPrice { get; set; }

    /// <summary>
    /// Gets or sets the URL of the product image.
    /// Optional property that can be null.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Gets or sets the caloric content of the product.
    /// Validated for non-empty and minimum value (0).
    /// </summary>
    public double Calories { get; set; }

    /// <summary>
    /// Gets or sets the fat content of the product.
    /// Validated for non-empty and minimum value (0).
    /// </summary>
    public double Fat { get; set; }

    /// <summary>
    /// Gets or sets the fiber content of the product.
    /// Validated for non-empty and minimum value (0).
    /// </summary>
    public double Fiber { get; set; }

    /// <summary>
    /// Gets or sets the protein content of the product.
    /// Validated for non-empty and minimum value (0).
    /// </summary>
    public double Protein { get; set; }

    /// <summary>
    /// Gets or sets the carbohydrate content of the product.
    /// Validated for non-empty and minimum value (0).
    /// </summary>
    public double Carbohydrates { get; set; }

    /// <summary>
    /// Gets or sets the file wrapper for the product image.
    /// Optional property that can be null.
    /// </summary>
    public FileWrapper? ImageFile { get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the AddProductForm class.
    /// Sets up all required validators for product properties.
    /// </summary>
    public AddProductForm()
    {
        AddProductNameValidator();
        AddProductUnitValidator();
        AddProductPriceValidator();
        AddProductCategoryValidator();
        AddProductDiscountPriceValidator();
        AddProductCaloriesValidator();
        AddProductFatValidator();
        AddProductFiberValidator();
        AddProductProteinValidator();
        AddProductCarbohydratesValidator();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds validation rules for the CategoryId property.
    /// Validates that the category ID is not empty.
    /// </summary>
    private void AddProductCategoryValidator()
    {
        AddValidator(nameof(CategoryId), (value) =>
        {
            return value.Validate(
                v => ValidationRules.IntRules.NotEmpty(v, "Category")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Name property.
    /// Validates that the product name is not empty and has minimum length of 2 characters.
    /// </summary>
    private void AddProductNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Product Name"),
                v => ValidationRules.StringRules.MinLength(v, 2, "Product Name")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Unit property.
    /// Validates that the unit is not empty and has minimum length of 1 character.
    /// </summary>
    private void AddProductUnitValidator()
    {
        AddValidator(nameof(Unit), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Unit"),
                v => ValidationRules.StringRules.MinLength(v, 1, "Unit")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Price property.
    /// Validates that the price is not empty and is not negative.
    /// </summary>
    private void AddProductPriceValidator()
    {
        AddValidator(nameof(Price), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Price"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Price")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the DiscountPrice property.
    /// Validates that the discount price is not negative.
    /// </summary>
    private void AddProductDiscountPriceValidator()
    {
        AddValidator(nameof(DiscountPrice), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.Min(v, 0, "Discount Price")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Calories property.
    /// Validates that the calories value is not empty and is not negative.
    /// </summary>
    private void AddProductCaloriesValidator()
    {
        AddValidator(nameof(Calories), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Calories"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Calories")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Fat property.
    /// Validates that the fat content is not empty and is not negative.
    /// </summary>
    private void AddProductFatValidator()
    {
        AddValidator(nameof(Fat), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Fat"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Fat")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Fiber property.
    /// Validates that the fiber content is not empty and is not negative.
    /// </summary>
    private void AddProductFiberValidator()
    {
        AddValidator(nameof(Fiber), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Fiber"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Fiber")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Protein property.
    /// Validates that the protein content is not empty and is not negative.
    /// </summary>
    private void AddProductProteinValidator()
    {
        AddValidator(nameof(Protein), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Protein"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Protein")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    /// <summary>
    /// Adds validation rules for the Carbohydrates property.
    /// Validates that the carbohydrates content is not empty and is not negative.
    /// </summary>
    private void AddProductCarbohydratesValidator()
    {
        AddValidator(nameof(Carbohydrates), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Carbohydrates"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Carbohydrates")
            ).Where(r => !r.IsValid).ToList();
        });
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Validates all properties of the form.
    /// Executes validation rules for all required product properties.
    /// </summary>
    /// <remarks>
    /// This method overrides the base ValidateAll method to provide
    /// form-specific validation logic for product data.
    /// </remarks>
    public override void ValidateAll()
    {
        ValidateProperty(nameof(Name), Name);
        ValidateProperty(nameof(Unit), Unit);
        ValidateProperty(nameof(Price), Price);
        ValidateProperty(nameof(CategoryId), CategoryId);
        ValidateProperty(nameof(Calories), Calories);
        ValidateProperty(nameof(Fat), Fat);
        ValidateProperty(nameof(Fiber), Fiber);
        ValidateProperty(nameof(Protein), Protein);
        ValidateProperty(nameof(Carbohydrates), Carbohydrates);
    }
    #endregion
}