using BistroQ.Domain.Contracts.Services.Realtime;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware, IRecipient<CheckoutRequestedMessage>
{
    public ProductListViewModel ProductListViewModel { get; }
    public OrderCartViewModel OrderCartViewModel { get; }

    private readonly ICheckoutRealTimeService _checkoutService;

    private readonly IMessenger _messenger;

    public ICommand AddProductToCartCommand { get; private set; }

    public HomePageViewModel()
    {
        ProductListViewModel = App.GetService<ProductListViewModel>();
        OrderCartViewModel = App.GetService<OrderCartViewModel>();
        _checkoutService = App.GetService<ICheckoutRealTimeService>();
        _messenger = App.GetService<IMessenger>();

        OrderCartViewModel.OrderStartedCommand = new RelayCommand<OrderViewModel>(OnOrderStarted);
        _messenger.RegisterAll(this);
    }

    private void OnOrderStarted(OrderViewModel order)
    {
    }

    public async Task OnNavigatedTo(object parameter)
    {
        try
        {
            await _checkoutService.StartAsync();
            await OrderCartViewModel.LoadExistingOrderAsync();
        }
        catch (Exception e)
        {
            if (e.Message != "Order not found")
            {
                await App.GetService<IDialogService>().ShowDialogAsync(new Microsoft.UI.Xaml.Controls.ContentDialog
                {
                    Title = "Error starting order",
                    Content = e.Message,
                    PrimaryButtonText = "OK",
                });
            }
        }
        finally
        {
            OrderCartViewModel.IsLoading = false;
        }
        _ = ProductListViewModel.LoadCategoriesAsync();
        _ = ProductListViewModel.LoadProductAsync();
    }

    public void Receive(CheckoutRequestedMessage message)
    {
        _checkoutService.NotifyCheckoutRequestedAsync(message.TableId ?? 0);
    }

    public Task OnNavigatedFrom()
    {
        OrderCartViewModel.Dispose();
        _checkoutService.StopAsync();
        _messenger.UnregisterAll(this);
        return Task.CompletedTask;
    }
}