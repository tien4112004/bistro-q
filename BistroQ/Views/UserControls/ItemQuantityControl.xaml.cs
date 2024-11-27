using BistroQ.Core.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;

namespace BistroQ.Views.UserControls;

public sealed partial class ItemQuantityControl : UserControl
{
    public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
        nameof(Item),
        typeof(OrderItem),
        typeof(ItemQuantityControl),
        new PropertyMetadata(null, OnItemChanged));

    public OrderItem Item
    {
        get => (OrderItem)GetValue(ItemProperty);
        set
        {
            SetValue(ItemProperty, value);
            UpdateVisualState();
        }
    }
    public ItemQuantityControl()
    {
        this.InitializeComponent();
    }

    private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ItemQuantityControl)d;
        var oldItem = e.OldValue as OrderItem;
        var newItem = e.NewValue as OrderItem;

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
        if (e.PropertyName == nameof(OrderItem.Quantity))
        {
            UpdateVisualState();
        }
    }

    private void UpdateVisualState()
    {
        if (Item == null) { return; }
        VisualStateManager.GoToState(this, Item.Quantity > 0 ? "InCart" : "NotInCart", true);
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        Item.Quantity = 1;
    }

    private void IncreaseButton_Click(object sender, RoutedEventArgs e)
    {
        Item.Quantity++;
    }

    private void DecreaseButton_Click(object sender, RoutedEventArgs e)
    {
        if (Item.Quantity > 0)
        {
            Item.Quantity--;
        }
    }
}
