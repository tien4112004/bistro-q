using BistroQ.Domain.Dtos.Products;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for the admin product interface.
/// Handles product data, selection state, and search functionality.
/// </summary>
/// <remarks>
/// Manages product data including:
/// - Search by name
/// - Selection tracking
/// - Data source management
/// </remarks>
public partial class AdminProductState : ObservableObject
{
    /// <summary>
    /// Gets or sets the query parameters for product collection.
    /// Triggers SearchText property updates when changed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private ProductCollectionQueryParams _query = new();

    /// <summary>
    /// Gets or sets the currently selected product.
    /// Affects CanEdit and CanDelete states.
    /// </summary>
    [ObservableProperty]
    private ProductViewModel? _selectedProduct;

    /// <summary>
    /// Gets or sets the collection of product data.
    /// Primary data source for product management interface.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ProductViewModel> source = new();

    /// <summary>
    /// Gets or sets whether data is currently being loaded.
    /// Used to control UI loading states.
    /// </summary>
    [ObservableProperty]
    private bool isLoading;

    /// <summary>
    /// Gets or sets the search text for filtering products.
    /// Directly maps to Query.Name with change notification.
    /// </summary>
    public string SearchText
    {
        get => Query.Name ?? string.Empty;
        set
        {
            if (Query.Name != value)
            {
                Query.Name = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets whether a product is selected and can be edited
    /// </summary>
    public bool CanEdit => SelectedProduct != null;

    /// <summary>
    /// Gets whether the selected product can be deleted
    /// </summary>
    public bool CanDelete => SelectedProduct != null;

    /// <summary>
    /// Resets the state to default values.
    /// Clears selection, source data, and query parameters.
    /// </summary>
    public void Reset()
    {
        SelectedProduct = null;
        Source.Clear();
        IsLoading = false;
        Query = new ProductCollectionQueryParams();
    }

    /// <summary>
    /// Returns to the first page of results.
    /// Used when search criteria change.
    /// </summary>
    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}