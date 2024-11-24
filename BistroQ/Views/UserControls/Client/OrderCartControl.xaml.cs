//using BistroQ.ViewModels.Client;
//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;

//namespace BistroQ.Views.UserControls.Client
//{
//    public sealed partial class OrderCartControl : UserControl
//    {
//        public DependencyProperty ViewModelProperty =
//            DependencyProperty.Register(
//                nameof(OrderCartViewModel),
//                typeof(OrderCartViewModel),
//                typeof(OrderCartControl),
//                new PropertyMetadata(null));

//        public OrderCartViewModel ViewModel
//        {
//            get => (OrderCartViewModel)GetValue(ViewModelProperty);
//            set => SetValue(ViewModelProperty, value);
//        }

//        public OrderCartControl()
//        {
//            this.InitializeComponent();
//        }

//        private void OrderCartSelector_SelectionChanged(object sender, SelectorBarSelectionChangedEventArgs e)
//        {
//            if (OrderCartSelector.SelectedItem == SelectorBarItemCart)
//            {
//                PanelContentControl.ContentTemplate = (DataTemplate)Resources["CartContentTemplate"];
//            }
//            else if (OrderCartSelector.SelectedItem == SelectorBarItemOrder)
//            {
//                PanelContentControl.ContentTemplate = (DataTemplate)Resources["OrderContentTemplate"];
//                PanelContentControl.Content = DataContext; // Ensure the DataContext is set for binding
//            }
//        }
//    }
//}

using BistroQ.ViewModels.Client;
using BistroQ.Views.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.UserControls.Client;

public class TabItemData
{
    public string Header { get; set; }
    public IconSource IconSource { get; set; }
    public Type PageType { get; set; }
}

public sealed partial class OrderCartControl : UserControl
{
    public DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(OrderCartViewModel),
            typeof(OrderCartViewModel),
            typeof(OrderCartControl),
            new PropertyMetadata(null));

    public OrderCartViewModel ViewModel
    {
        get => (OrderCartViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public OrderCartControl()
    {
        this.InitializeComponent();
    }

    private void OrderCartSelector_SelectionChanged(object sender, SelectorBarSelectionChangedEventArgs e)
    {
        if (OrderCartSelector.SelectedItem == SelectorBarItemCart)
        {
            PanelContentControl.Content = new CartPage();
        }
        else if (OrderCartSelector.SelectedItem == SelectorBarItemOrder)
        {
            PanelContentControl.Content = new OrderPage();
        }
    }
}
