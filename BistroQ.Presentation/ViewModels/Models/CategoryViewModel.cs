using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Models;

/// <summary>
/// ViewModel representing a product category.
/// </summary>
public partial class CategoryViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets the category name
    /// </summary>
    [ObservableProperty]
    private string? _name;

    /// <summary>
    /// Gets or sets the unique identifier for the category
    /// </summary>
    [ObservableProperty]
    private int? _categoryId;
}