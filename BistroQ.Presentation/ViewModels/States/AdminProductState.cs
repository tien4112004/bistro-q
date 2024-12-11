using BistroQ.Domain.Dtos.Products;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.States;

public partial class AdminProductState : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SearchText))]
    private ProductCollectionQueryParams _query = new();

    [ObservableProperty]
    private ProductViewModel? _selectedProduct;

    [ObservableProperty]
    private ObservableCollection<ProductViewModel> source = new();

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

    public bool CanEdit => SelectedProduct != null;
    public bool CanDelete => SelectedProduct != null;

    public void Reset()
    {
        SelectedProduct = null;
        Source.Clear();
        IsLoading = false;
        Query = new ProductCollectionQueryParams();
    }

    public void ReturnToFirstPage()
    {
        Query.Page = 1;
    }
}