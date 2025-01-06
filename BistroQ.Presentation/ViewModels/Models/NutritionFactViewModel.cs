using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing nutritional information for a product.
/// </summary>
public partial class NutritionFactViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the caloric content
    /// </summary>
    [ObservableProperty]
    private string? _calories;

    /// <summary>
    /// Gets or sets the fat content
    /// </summary>
    [ObservableProperty]
    private string? _fat;

    /// <summary>
    /// Gets or sets the fiber content
    /// </summary>
    [ObservableProperty]
    private string? _fiber;

    /// <summary>
    /// Gets or sets the protein content
    /// </summary>
    [ObservableProperty]
    private string? _protein;

    /// <summary>
    /// Gets or sets the carbohydrates content
    /// </summary>
    [ObservableProperty]
    private string? _carbohydrates;
}