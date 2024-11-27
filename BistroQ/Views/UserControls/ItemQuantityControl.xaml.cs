using BistroQ.Core.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.UserControls
{
    public sealed partial class ItemQuantityControl : UserControl
    {
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            nameof(Item),
            typeof(OrderItem),
            typeof(ItemQuantityControl),
            new PropertyMetadata(null));

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
}
