using BistroQ.Domain.Dtos.Tables;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class AdminTableState : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private TableCollectionQueryParams _query = new();

    [ObservableProperty]
    private TableViewModel? _selectedTable;

    [ObservableProperty]
    private ObservableCollection<TableViewModel> source = new();

    [ObservableProperty]
    private bool isLoading;

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

    public bool CanEdit => SelectedTable != null;
    public bool CanDelete => SelectedTable != null;

    public void Reset()
    {
        SelectedTable = null;
        Source.Clear();
        IsLoading = false;
        Query = new TableCollectionQueryParams();
    }

    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}