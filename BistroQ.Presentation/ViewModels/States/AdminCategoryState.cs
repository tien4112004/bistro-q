using BistroQ.Domain.Dtos.Category;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class AdminCategoryState : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private CategoryCollectionQueryParams _query = new();

    [ObservableProperty]
    private CategoryViewModel? _selectedCategory;

    [ObservableProperty]
    private ObservableCollection<CategoryViewModel> source = new();

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

    public bool CanEdit => SelectedCategory != null;
    public bool CanDelete => SelectedCategory != null;

    public void Reset()
    {
        SelectedCategory = null;
        Source.Clear();
        IsLoading = false;
        Query = new CategoryCollectionQueryParams();
    }

    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}