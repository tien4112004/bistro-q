using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Realtime;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware, IRecipient<CheckoutRequestedMessage>
{
    [ObservableProperty]
    private bool _isShowingPayment;

    [ObservableProperty]
    private string _paymentData;

    public ProductListViewModel ProductListViewModel { get; }
    public OrderCartViewModel OrderCartViewModel { get; }

    private readonly ICheckoutRealTimeService _checkoutService;

    private readonly IMessenger _messenger;

    private readonly IDialogService _dialogService;

    private readonly DispatcherQueue _dispatcherQueue;

    public ICommand AddProductToCartCommand { get; private set; }

    public ICommand CancelPaymentCommand { get; }

    public HomePageViewModel()
    {
        ProductListViewModel = App.GetService<ProductListViewModel>();
        OrderCartViewModel = App.GetService<OrderCartViewModel>();
        _checkoutService = App.GetService<ICheckoutRealTimeService>();
        _dialogService = App.GetService<IDialogService>();
        _messenger = App.GetService<IMessenger>();

        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        CancelPaymentCommand = new AsyncRelayCommand(CancelPaymentAsync);

        _checkoutService.OnCheckoutInitiated += (paymentData) =>
        {
            _dispatcherQueue.TryEnqueue(() =>
            {
                PaymentData = paymentData;
                IsShowingPayment = true;
            });
        };

        _checkoutService.OnCheckoutCompleted += () =>
        {
            _dispatcherQueue.TryEnqueue(async () =>
            {
                const int LOGOUT_DELAY_SECONDS = 5;
                var remainingSeconds = LOGOUT_DELAY_SECONDS;
                var dialog = new ContentDialog
                {
                    Title = "Session Ending",
                    Content = $"Thank you for dining with us! The application will log out in {remainingSeconds} seconds.",
                    PrimaryButtonText = "OK",
                    DefaultButton = ContentDialogButton.Primary
                };

                var timer = _dispatcherQueue.CreateTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, e) =>
                {
                    remainingSeconds--;
                    dialog.Content = $"Thank you for dining with us! The application will log out in {remainingSeconds} seconds.";

                    if (remainingSeconds <= 0)
                    {
                        timer.Stop();
                        dialog.Hide();
                    }
                };
                timer.Start();

                _ = _dialogService.ShowDialogAsync(dialog);

                await Task.Delay(TimeSpan.FromSeconds(LOGOUT_DELAY_SECONDS));

                // Logout
                App.MainWindow.Hide();
                await App.GetService<IAuthService>().LogoutAsync();

                var size = App.MainWindow.AppWindow.Size;
                var position = App.MainWindow.AppWindow.Position;

                var loginWindow = new LoginWindow();
                loginWindow.Activate();
                loginWindow.MoveAndResize(position.X, position.Y, size.Width, size.Height);

                await Task.Delay(1000);
                App.MainWindow.Close();
            });
        };

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
                await _dialogService.ShowDialogAsync(new ContentDialog
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
        _ = ProductListViewModel.LoadProductsAsync();
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

    private async Task CancelPaymentAsync()
    {
        var dialog = new ContentDialog
        {
            Title = "Cancel Payment",
            Content = "Are you sure you want to cancel this payment?",
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No",
            SecondaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style
        };

        var result = await _dialogService.ShowDialogAsync(dialog);
        if (result == ContentDialogResult.Primary)
        {
            IsShowingPayment = false;
            PaymentData = null;
        }
    }
}