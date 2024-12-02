
using System.Diagnostics;
using BistroQ.Domain.Models.Entities;
using BistroQ.Presentation.ViewModels.Client;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class OrderCartControl : UserControl
{
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
    public event EventHandler<IEnumerable<OrderItem>> OrderRequested;
    public event EventHandler CheckoutRequested;

    public OrderCartControl()
    {
        this.InitializeComponent();
        this.Loaded += OrderCartControl_Loaded;
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
            cartControl.OrderRequested += OrderCartControl_OrderRequested;   // Don't care the name of the event pls...
        }
        else if (OrderCartSelector.SelectedItem == SelectorBarItemOrder)
        {
            var orderControl = new OrderControl(ViewModel);
            PanelContentControl.Content = orderControl;

        }
    }

    private void OrderCartControl_OrderRequested(object sender, IEnumerable<OrderItemViewModel> orderItems)
    {
        //if (orderItems == null)
        //{
        //    return;
        //}
        Debug.WriteLine(orderItems);

        // call order here
        Debug.WriteLine("Call api order here");
    }
}