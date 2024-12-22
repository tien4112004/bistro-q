using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class ProductViewModel : ObservableObject
{
    [ObservableProperty]
    private int? _productId;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private decimal? _price;

    [ObservableProperty]
    private string _unit = string.Empty;

    [ObservableProperty]
    private decimal? _discountPrice;

    [ObservableProperty]
    private string? _imageUrl;

    [ObservableProperty]
    private int? _imageId;

    [ObservableProperty]
    private CategoryViewModel? _category;

    [ObservableProperty]
    private NutritionFactViewModel _nutritionFact;

    public bool HasDiscount => _discountPrice.Value != 0;
    public bool NotHasDiscount => !HasDiscount;
}