using BistroQ.Presentation.ViewModels.KitchenOrder;
using Microsoft.UI.Xaml.Controls;


namespace BistroQ.Presentation.Views.KitchenOrder;

public sealed partial class KitchenOrderPage : Page
{
    public KitchenOrderViewModel ViewModel { get; }
    public KitchenOrderPage()
    {
        ViewModel = App.GetService<KitchenOrderViewModel>();
        InitializeComponent();
        this.Unloaded += (s, e) => ViewModel.Dispose();
    }
}
