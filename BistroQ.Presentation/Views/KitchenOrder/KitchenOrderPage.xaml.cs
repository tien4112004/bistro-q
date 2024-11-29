using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.KitchenOrder;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;


namespace BistroQ.Presentation.Views.KitchenOrder;

public sealed partial class KitchenOrderPage : Page
{
    public KitchenOrderViewModel ViewModel { get; }
    public KitchenOrderPage()
    {
        ViewModel = App.GetService<KitchenOrderViewModel>();
        InitializeComponent();
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        var button = sender as UIElement;
        if (button != null)
        {
            button.ChangeCursor(CursorType.Hand);
        }
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        var button = sender as UIElement;

        if (button != null)
        {
            button.ChangeCursor(CursorType.Arrow);
        }
    }
}
