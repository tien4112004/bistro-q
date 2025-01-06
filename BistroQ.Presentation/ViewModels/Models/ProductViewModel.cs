using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing a product available for order.
/// </summary>
/// <remarks>
/// Includes product details, pricing information, and nutritional facts.
/// </remarks>
public partial class ProductViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the unique identifier for the product
    /// </summary>
    [ObservableProperty]
    private int? _productId;

    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    [ObservableProperty]
    private string _name = string.Empty;

    /// <summary>
    /// Gets or sets the regular price
    /// </summary>
    [ObservableProperty]
    private decimal? _price;

    /// <summary>
    /// Gets or sets the unit of measurement
    /// </summary>
    [ObservableProperty]
    private string _unit = string.Empty;

    /// <summary>
    /// Gets or sets the discounted price, if applicable
    /// </summary>
    [ObservableProperty]
    private decimal? _discountPrice;

    /// <summary>
    /// Gets or sets the URL to the product image
    /// </summary>
    [ObservableProperty]
    private string? _imageUrl;

    /// <summary>
    /// Gets or sets the image identifier
    /// </summary>
    [ObservableProperty]
    private int? _imageId;

    /// <summary>
    /// Gets or sets the product category information
    /// </summary>
    [ObservableProperty]
    private CategoryViewModel? _category;

    /// <summary>
    /// Gets or sets the nutritional information
    /// </summary>
    [ObservableProperty]
    private NutritionFactViewModel _nutritionFact;

    /// <summary>
    /// Gets whether the product has an active discount
    /// </summary>
    public bool HasDiscount => _discountPrice.Value != 0;
}