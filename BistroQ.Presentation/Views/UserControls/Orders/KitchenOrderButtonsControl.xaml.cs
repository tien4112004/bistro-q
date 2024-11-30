using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class KitchenOrderButtonsControl : UserControl
{
    private readonly IMessenger _messenger = App.GetService<IMessenger>();

    public IEnumerable<KitchenOrderItemViewModel> Items { get; set; } = new List<KitchenOrderItemViewModel>();

    public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
        nameof(Items),
        typeof(IEnumerable<KitchenOrderItemViewModel>),
        typeof(KitchenOrderButtonsControl),
        new PropertyMetadata(null));

    public KitchenOrderButtonsControl()
    {
        this.InitializeComponent();
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

    private void CompleteButton_Click(object sender, RoutedEventArgs e)
    {
        _messenger.Send(new KitchenActionMessage(Items.Select((item) => item.OrderItemId), Enums.KitchenAction.Complete));
    }

    private void MoveButton_Click(object sender, RoutedEventArgs e)
    {
        _messenger.Send(new KitchenActionMessage(Items.Select((item) => item.OrderItemId), Enums.KitchenAction.Move));
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _messenger.Send(new KitchenActionMessage(Items.Select((item) => item.OrderItemId), Enums.KitchenAction.Cancel));
    }
}
