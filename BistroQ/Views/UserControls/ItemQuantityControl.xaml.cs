using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace BistroQ.Views.UserControls
{
    public sealed partial class ItemQuantityControl : UserControl
    {
        public static readonly DependencyProperty QuantityProperty =
            DependencyProperty.Register(nameof(Quantity), typeof(int), typeof(ItemQuantityControl),
                new PropertyMetadata(0, OnQuantityChanged));

        public static readonly DependencyProperty ProductIdProperty =
            DependencyProperty.Register(nameof(ProductId), typeof(string), typeof(ItemQuantityControl),
                new PropertyMetadata(string.Empty));

        public event TypedEventHandler<ItemQuantityControl, QuantityChangedEventArgs> QuantityChanged;

        public int Quantity
        {
            get => (int)GetValue(QuantityProperty);
            set => SetValue(QuantityProperty, value);
        }

        public string ProductId
        {
            get => (string)GetValue(ProductIdProperty);
            set => SetValue(ProductIdProperty, value);
        }

        public ItemQuantityControl()
        {
            this.InitializeComponent();
            UpdateVisualState();
        }

        private static void OnQuantityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ItemQuantityControl)d;
            control.UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            VisualStateManager.GoToState(this, Quantity > 0 ? "InCart" : "NotInCart", true);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Quantity = 1;
            QuantityChanged?.Invoke(this, new QuantityChangedEventArgs(ProductId, Quantity));
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Quantity++;
            QuantityChanged?.Invoke(this, new QuantityChangedEventArgs(ProductId, Quantity));
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity > 0)
            {
                Quantity--;
                QuantityChanged?.Invoke(this, new QuantityChangedEventArgs(ProductId, Quantity));
            }
        }
    }

    public class QuantityChangedEventArgs : EventArgs
    {
        public string ProductId { get; }
        public int NewQuantity { get; }

        public QuantityChangedEventArgs(string productId, int newQuantity)
        {
            ProductId = productId;
            NewQuantity = newQuantity;
        }
    }
}
