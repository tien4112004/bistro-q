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

    public override void ValidateAll()
    {
        ValidateProperty(nameof(Name), Name);
        ValidateProperty(nameof(Unit), Unit);
        ValidateProperty(nameof(Price), Price);
        ValidateProperty(nameof(CategoryId), CategoryId);
    }
}