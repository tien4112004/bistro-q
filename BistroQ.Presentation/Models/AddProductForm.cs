using BistroQ.Presentation.Validation;

namespace BistroQ.Presentation.Models;

public class AddProductForm : ValidatorBase
{
    public AddProductForm()
    {
        AddProductNameValidator();
        AddProductUnitValidator();
        AddProductPriceValidator();
        AddProductCategoryValidator();
    }

    public int? ProductId { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string? ImageUrl { get; set; }
    public double Calories { get; set; }
    public double Fat { get; set; }
    public double Fiber { get; set; }
    public double Protein { get; set; }
    public double Carbohydrates { get; set; }

    public FileWrapper? ImageFile { get; set; }

    public void AddProductCategoryValidator()
    {
        AddValidator(nameof(CategoryId), (value) =>
        {
            return value.Validate(
                v => ValidationRules.IntRules.NotEmpty(v, "Category")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductNameValidator()
    {
        AddValidator(nameof(Name), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Product Name"),
                v => ValidationRules.StringRules.MinLength(v, 2, "Product Name")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductUnitValidator()
    {
        AddValidator(nameof(Unit), (value) =>
        {
            return value.Validate(
                v => ValidationRules.StringRules.NotEmpty(v, "Unit"),
                v => ValidationRules.StringRules.MinLength(v, 1, "Unit")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductPriceValidator()
    {
        AddValidator(nameof(Price), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Price"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Price")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductDiscountPriceValidator()
    {
        AddValidator(nameof(DiscountPrice), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.Min(v, 0, "Discount Price")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductCaloriesValidator()
    {
        AddValidator(nameof(Calories), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Calories"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Calories")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductFatValidator()
    {
        AddValidator(nameof(Fat), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Fat"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Fat")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductFiberValidator()
    {
        AddValidator(nameof(Fiber), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Fiber"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Fiber")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductProteinValidator()
    {
        AddValidator(nameof(Protein), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Protein"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Protein")
            ).Where(r => !r.IsValid).ToList();
        });
    }

    public void AddProductCarbohydratesValidator()
    {
        AddValidator(nameof(Carbohydrates), (value) =>
        {
            return value.Validate(
                v => ValidationRules.DecimalRules.NotEmpty(v, "Carbohydrates"),
                v => ValidationRules.DecimalRules.Min(v, 0, "Carbohydrates")
            ).Where(r => !r.IsValid).ToList();
        });
    }

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
}