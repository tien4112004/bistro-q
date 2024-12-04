using System.ComponentModel;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls;

public sealed partial class ItemQuantityControl : UserControl
{
    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
        nameof(Item),
        typeof(OrderItemViewModel),
        typeof(ItemQuantityControl),
        new PropertyMetadata(null, OnItemChanged));

    public OrderItemViewModel Item
    {
        get => (OrderItemViewModel)GetValue(ItemProperty);
        set
        {
            SetValue(ItemProperty, value);
            UpdateVisualState();
        }
    }
    public IRelayCommand IncreaseQuantityCommand { get; }
    public IRelayCommand DecreaseQuantityCommand { get; }

    public ItemQuantityControl()
    {
        this.InitializeComponent();

        IncreaseQuantityCommand = new RelayCommand(Increase);
        DecreaseQuantityCommand = new RelayCommand(Decrease, CanDecrease);
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ItemQuantityControl)d;
        var oldItem = e.OldValue as OrderItemViewModel;
        var newItem = e.NewValue as OrderItemViewModel;

        if (oldItem != null)
        {
            oldItem.PropertyChanged -= control.Item_PropertyChanged;
        }

        if (newItem != null)
        {
            newItem.PropertyChanged += control.Item_PropertyChanged;
            control.UpdateVisualState();
        }
    }

    private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(OrderItemViewModel.Quantity))
        {
            UpdateVisualState();
        }
    }

    private void UpdateVisualState()
    {
        if (Item == null) { return; }
        VisualStateManager.GoToState(this, Item.Quantity > 0 ? "InCart" : "NotInCart", true);
        DecreaseQuantityCommand.NotifyCanExecuteChanged();
    }

    private void Increase() => Item.Quantity++;

    private bool CanDecrease() => Item?.Quantity > 1;
    private void Decrease()
    {
        if (Item.Quantity > 1)
        {
            Item.Quantity--;
        }
        DecreaseQuantityCommand.NotifyCanExecuteChanged();
    }
}