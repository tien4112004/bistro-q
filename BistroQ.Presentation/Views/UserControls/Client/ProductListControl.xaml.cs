using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class ProductListControl : UserControl
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel),
        typeof(ProductListViewModel),
        typeof(ProductListControl),
        new PropertyMetadata(null));

    public ProductListViewModel ViewModel
    {
        get => (ProductListViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public event EventHandler<ProductViewModel> AddProductToCart;
    public event EventHandler<ProductViewModel> ProductSelected;

    public ProductListControl()
    {
        this.InitializeComponent();
        this.Loaded += ProductListControl_Loaded;
    }

    private void ProductListControl_Loaded(object sender, RoutedEventArgs e)
    {
        DataContext = ViewModel;
    }

    private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel != null && e.AddedItems.Count > 0)
        {
            ViewModel.SelectedCategory = e.AddedItems[0] as CategoryViewModel;
            if (ViewModel.ChangeCategoryCommand.CanExecute(ViewModel.SelectedCategory))
            {
                ViewModel.ChangeCategoryCommand.Execute(ViewModel.SelectedCategory);
            }
        }
    }

    private void AddToCart(object sender, ProductViewModel product)
    {
        AddProductToCart?.Invoke(this, product);
    }

    private void ScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        const double SCROLL_SPEED = 1.25;
        var scrollViewer = (ScrollViewer)sender;
        scrollViewer.ChangeView(
            scrollViewer.HorizontalOffset - e.Delta.Translation.X * SCROLL_SPEED,
            null,
            null,
            true);
    }
}