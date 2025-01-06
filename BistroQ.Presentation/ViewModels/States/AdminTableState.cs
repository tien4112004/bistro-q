using BistroQ.Domain.Dtos.Tables;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for the admin table interface.
/// Handles table data, selection state, and search functionality.
/// </summary>
/// <remarks>
/// Manages table data including:
/// - Search by zone name
/// - Selection tracking
/// - Data source management
/// </remarks>
public partial class AdminTableState : ObservableObject
{
    /// <summary>
    /// Gets or sets the query parameters for table collection.
    /// Triggers SearchText property updates when changed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private TableCollectionQueryParams _query = new();

    /// <summary>
    /// Gets or sets the currently selected table.
    /// Affects CanEdit and CanDelete states.
    /// </summary>
    [ObservableProperty]
    private TableViewModel? _selectedTable;

    /// <summary>
    /// Gets or sets the collection of table data.
    /// Primary data source for table management interface.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<TableViewModel> source = new();

    /// <summary>
    /// Gets or sets whether data is currently being loaded.
    /// Used to control UI loading states.
    /// </summary>
    [ObservableProperty]
    private bool isLoading;

    /// <summary>
    /// Gets or sets the search text for filtering tables by zone name.
    /// Directly maps to Query.ZoneName with change notification.
    /// </summary>
    public string SearchText
    {
        get => Query.ZoneName ?? string.Empty;
        set
        {
            if (Query.ZoneName != value)
            {
                Query.ZoneName = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets whether a table is selected and can be edited
    /// </summary>
    public bool CanEdit => SelectedTable != null;

    /// <summary>
    /// Gets whether the selected table can be deleted
    /// </summary>
    public bool CanDelete => SelectedTable != null;

    /// <summary>
    /// Resets the state to default values.
    /// Clears selection, source data, and query parameters.
    /// </summary>
    public void Reset()
    {
        SelectedTable = null;
        Source.Clear();
        IsLoading = false;
        Query = new TableCollectionQueryParams();
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