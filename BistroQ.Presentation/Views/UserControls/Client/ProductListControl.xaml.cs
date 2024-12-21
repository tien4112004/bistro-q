using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Messaging;
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

    private IMessenger _messenger = App.GetService<IMessenger>();

    public ProductListViewModel ViewModel
    {
        get => (ProductListViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public event EventHandler<ProductViewModel> ProductSelected;

    public ProductListControl()
    {
        this.InitializeComponent();
        this.Loaded += ProductListControl_Loaded;
        _messenger.RegisterAll(this);
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

    private void HorizontalScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        const double SCROLL_SPEED = 1.25;
        var scrollViewer = (ScrollViewer)sender;
        scrollViewer.ChangeView(
            scrollViewer.HorizontalOffset - e.Delta.Translation.X * SCROLL_SPEED,
            null,
            null,
            true);
    }

    private void VerticalScrollViewer_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
        const double SCROLL_SPEED = 1.25;
        var scrollViewer = (ScrollViewer)sender;
        scrollViewer.ChangeView(
            null,
            scrollViewer.VerticalOffset - e.Delta.Translation.Y * SCROLL_SPEED,
            null,
            true);
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }

    private async void SingleProductControl_ProductClicked(object sender, ProductViewModel e)
    {
        var _dialogService = App.GetService<IDialogService>();

        if (_dialogService != null)
        {
            var productDetailControl = new ProductDetailControl(e);
            var productDetailDialog = new ContentDialog
            {
                Content = productDetailControl,
                Title = e.Name,
                CloseButtonText = "Close",
                PrimaryButtonText = "Add to cart",
            };
            await _dialogService.ShowDialogAsync(productDetailDialog);
            //if (dialogResult == DialogResult.OK)
            //{
            //    // Handle the dialog result if needed
            //}
        }
    }
}