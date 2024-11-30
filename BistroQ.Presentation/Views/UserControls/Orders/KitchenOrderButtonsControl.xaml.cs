using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels.KitchenOrder;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class KitchenOrderButtonsControl : UserControl
{
    public KitchenOrderButtonsViewModel ViewModel { get; set; }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel),
        typeof(KitchenOrderButtonsViewModel),
        typeof(KitchenOrderButtonsControl),
        new PropertyMetadata(null));

    public KitchenOrderButtonsControl()
    {
        InitializeComponent();
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
