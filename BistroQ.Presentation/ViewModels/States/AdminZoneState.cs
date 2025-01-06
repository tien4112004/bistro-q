using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

/// <summary>
/// State management class for the admin zone interface.
/// Handles zone data, selection state, and search functionality.
/// </summary>
/// <remarks>
/// Manages zone data including:
/// - Search by name
/// - Selection tracking
/// - Data source management
/// </remarks>
public partial class AdminZoneState : ObservableObject
{
    /// <summary>
    /// Gets or sets the query parameters for zone collection.
    /// Triggers SearchText property updates when changed.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private ZoneCollectionQueryParams _query = new();

    /// <summary>
    /// Gets or sets the currently selected zone.
    /// Affects CanEdit and CanDelete states.
    /// </summary>
    [ObservableProperty]
    private ZoneViewModel? _selectedZone;

    /// <summary>
    /// Gets or sets the collection of zone data.
    /// Primary data source for zone management interface.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> source = new();

    /// <summary>
    /// Gets or sets whether data is currently being loaded.
    /// Used to control UI loading states.
    /// </summary>
    [ObservableProperty]
    private bool isLoading;

    /// <summary>
    /// Gets or sets the search text for filtering zones.
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
    /// Gets whether a zone is selected and can be edited
    /// </summary>
    public bool CanEdit => SelectedZone != null;

    /// <summary>
    /// Gets whether the selected zone can be deleted
    /// </summary>
    public bool CanDelete => SelectedZone != null;

    /// <summary>
    /// Resets the state to default values.
    /// Clears selection, source data, and query parameters.
    /// </summary>
    public void Reset()
    {
        SelectedZone = null;
        Source.Clear();
        IsLoading = false;
        Query = new ZoneCollectionQueryParams();
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