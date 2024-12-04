using BistroQ.Domain.Dtos.Zones;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class AdminZoneState : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private ZoneCollectionQueryParams _query = new();

    [ObservableProperty]
    private ZoneViewModel? _selectedZone;

    [ObservableProperty]
    private ObservableCollection<ZoneViewModel> source = new();

    [ObservableProperty]
    private bool isLoading;

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

    public bool CanEdit => SelectedZone != null;
    public bool CanDelete => SelectedZone != null;

    public void Reset()
    {
        SelectedZone = null;
        Source.Clear();
        IsLoading = false;
        Query = new ZoneCollectionQueryParams();
    }

    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}
