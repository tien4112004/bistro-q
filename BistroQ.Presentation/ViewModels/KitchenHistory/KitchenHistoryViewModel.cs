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

    public async Task OnNavigatedTo(object parameter)
    {
        await OrderItemGridViewModel.LoadItemsAsync();
    }

    public Task OnNavigatedFrom()
    {
        OrderItemGridViewModel.Dispose();
        return Task.CompletedTask;
    }
}
