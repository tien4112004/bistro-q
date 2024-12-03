using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

public partial class ProductViewModel : ObservableObject
{
    [ObservableProperty]
    private int? _productId;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private int? _categoryId;

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
}