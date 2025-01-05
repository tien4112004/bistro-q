using BistroQ.Domain.Dtos.Category;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for the admin category interface.
/// Handles category data, selection state, and search functionality.
/// </summary>
/// <remarks>
/// Manages category data including:
/// - Search by name
/// - Selection tracking
/// - Data source management
/// </remarks>
public partial class AdminCategoryState : ObservableObject
{
    /// <summary>
    /// Gets or sets the query parameters for category collection.
    /// Triggers SearchText property updates when changed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private CategoryCollectionQueryParams _query = new();

    /// <summary>
    /// Gets or sets the currently selected category.
    /// Affects CanEdit and CanDelete states.
    /// </summary>
    [ObservableProperty]
    private CategoryViewModel? _selectedCategory;

    /// <summary>
    /// Gets or sets the collection of category data.
    /// Primary data source for category management interface.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<CategoryViewModel> source = new();

    /// <summary>
    /// Gets or sets whether data is currently being loaded.
    /// Used to control UI loading states.
    /// </summary>
    [ObservableProperty]
    private bool isLoading;

    /// <summary>
    /// Gets or sets the search text for filtering categories.
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
    /// Gets whether a category is selected and can be edited
    /// </summary>
    public bool CanEdit => SelectedCategory != null;

    /// <summary>
    /// Gets whether the selected category can be deleted
    /// </summary>
    public bool CanDelete => SelectedCategory != null;

    /// <summary>
    /// Resets the state to default values.
    /// Clears selection, source data, and query parameters.
    /// </summary>
    public void Reset()
    {
        SelectedCategory = null;
        Source.Clear();
        IsLoading = false;
        Query = new CategoryCollectionQueryParams();
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