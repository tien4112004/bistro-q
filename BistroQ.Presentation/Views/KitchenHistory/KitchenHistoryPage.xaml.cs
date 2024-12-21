using BistroQ.Domain.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.KitchenHistory;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml.Controls;


namespace BistroQ.Presentation.Views.KitchenHistory;

public sealed partial class KitchenHistoryPage : Page
{
    public KitchenHistoryViewModel ViewModel { get; set; }

    private List<(OrderItemStatus Status, string DisplayName)> _statusItems = new()
    {
        (OrderItemStatus.Pending, "Pending"),
        (OrderItemStatus.InProgress, "In Progress"),
        (OrderItemStatus.Completed, "Completed"),
        (OrderItemStatus.Cancelled, "Cancelled")
    };

    public KitchenHistoryPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<KitchenHistoryViewModel>();
        for (var i = 0; i < _statusItems.Count; i++)
        {
            Segmented.Items.Add(new SegmentedItem
            {
                Content = _statusItems[i].DisplayName,
            });
        }
    }

    private void Segmented_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ViewModel == null)
        {
            return;
        }

        var type = _statusItems[Segmented.SelectedIndex].Status;

        if (ViewModel.OrderItemGridViewModel.State.Query.Status == type.ToString())
        {
            return;
        }
        App.GetService<IMessenger>().Send(new OrderGridStatusChangedMessage(type));
    }
}
