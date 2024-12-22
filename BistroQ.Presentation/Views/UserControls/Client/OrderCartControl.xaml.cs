
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Client;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class OrderCartControl :
    UserControl,
    IRecipient<OrderSucceededMessage>,
    IRecipient<AddProductToCartMessage>,
    IDisposable
{
    private IMessenger _messenger = App.GetService<IMessenger>();

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(OrderCartViewModel),
            typeof(OrderCartControl),
            new PropertyMetadata(null, OnViewModelChanged));

    public OrderCartViewModel ViewModel
    {
        get => (OrderCartViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public event EventHandler<OrderCartViewModel> ViewModelChanged;
    public OrderCartControl()
    {
        this.InitializeComponent();
        this.Loaded += OrderCartControl_Loaded;
        _messenger.RegisterAll(this);
    }

    private void OrderCartControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (ViewModel == null)
        {
            ViewModel = App.GetService<OrderCartViewModel>();
        }
        DataContext = ViewModel;
        OrderCartSelector.SelectedItem = SelectorBarItemCart;
    }

    private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (OrderCartControl)d;
        control.OnViewModelChanged((OrderCartViewModel)e.NewValue);
    }

    private void OnViewModelChanged(OrderCartViewModel newViewModel)
    {
        ViewModelChanged?.Invoke(this, newViewModel);
    }

    private void OrderCartSelector_SelectionChanged(object sender, SelectorBarSelectionChangedEventArgs e)
    {
        if (OrderCartSelector.SelectedItem == SelectorBarItemCart)
        {
            var cartControl = new CartControl(ViewModel);
            PanelContentControl.Content = cartControl;
        }
        else if (OrderCartSelector.SelectedItem == SelectorBarItemOrder)
        {
            var orderControl = new OrderControl(ViewModel);
            PanelContentControl.Content = orderControl;
        }
    }

    public void Receive(OrderSucceededMessage message)
    {
        OrderCartSelector.SelectedItem = SelectorBarItemOrder;
    }

    public void Receive(AddProductToCartMessage message)
    {
        if (OrderCartSelector.SelectedItem != SelectorBarItemCart)
        {
            OrderCartSelector.SelectedItem = SelectorBarItemCart;
        }
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }
}