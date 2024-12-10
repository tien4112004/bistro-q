using BistroQ.Presentation.Contracts.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.KitchenHistory;

public partial class KitchenHistoryViewModel : ObservableObject, INavigationAware
{
    public OrderItemGridViewModel OrderItemGridViewModel { get; set; }

    public KitchenHistoryViewModel(OrderItemGridViewModel orderItemGridViewModel)
    {
        OrderItemGridViewModel = orderItemGridViewModel;
    }

    public void OnNavigatedTo(object parameter)
    {
        OrderItemGridViewModel.LoadItemsAsync();
    }

    public void OnNavigatedFrom()
    {
        OrderItemGridViewModel.Dispose();
    }
}
