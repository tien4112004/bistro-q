using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Client
{
    public sealed partial class OrderCartControl : UserControl
    {
        public DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(OrderCartViewModel),
                typeof(OrderCartViewModel),
                typeof(OrderCartViewModel),
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
                PanelContentControl.ContentTemplate = (DataTemplate)Resources["CartContentTemplate"];
            }
            else if (OrderCartSelector.SelectedItem == SelectorBarItemOrder)
            {
                PanelContentControl.ContentTemplate = (DataTemplate)Resources["OrderContentTemplate"];
                PanelContentControl.Content = DataContext; // Ensure the DataContext is set for binding
            }
        }
    }
}
