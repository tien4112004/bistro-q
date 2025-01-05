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

/// <summary>
/// ViewModel for the home page of the client application.
/// Handles product listing, order cart management, and payment processing.
/// </summary>
/// <remarks>
/// Implements:
/// - ObservableRecipient for MVVM pattern
/// - INavigationAware for page navigation
/// - IRecipient for handling checkout messages
/// </remarks>
public partial class HomePageViewModel : ObservableRecipient, INavigationAware, IRecipient<CheckoutRequestedMessage>
{
    #region Private Fields
    /// <summary>
    /// Flag indicating whether the payment UI is currently displayed.
    /// </summary>
    [ObservableProperty]
    private bool _isShowingPayment;

    /// <summary>
    /// Payment data received from the checkout service.
    /// </summary>
    [ObservableProperty]
    private string _paymentData;

    /// <summary>
    /// Service for handling real-time checkout operations.
    /// </summary>
    private readonly ICheckoutRealTimeService _checkoutService;

    /// <summary>
    /// Messenger service for handling application-wide messages.
    /// </summary>
    private readonly IMessenger _messenger;

    /// <summary>
    /// Service for displaying dialogs to the user.
    /// </summary>
    private readonly IDialogService _dialogService;

    /// <summary>
    /// Dispatcher queue for handling UI thread operations.
    /// </summary>
    private readonly DispatcherQueue _dispatcherQueue;
    #endregion

    #region Public Properties
    /// <summary>
    /// ViewModel for managing the product list display.
    /// </summary>
    public ProductListViewModel ProductListViewModel { get; }

    /// <summary>
    /// ViewModel for managing the order cart.
    /// </summary>
    public OrderCartViewModel OrderCartViewModel { get; }
    #endregion

    #region Commands
    /// <summary>
    /// Command for adding products to the cart.
    /// </summary>
    public ICommand AddProductToCartCommand { get; private set; }

    /// <summary>
    /// Command for canceling the current payment process.
    /// </summary>
    public ICommand CancelPaymentCommand { get; }
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the HomePageViewModel class.
    /// Sets up services, commands, and event handlers for checkout process.
    /// </summary>
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
    #endregion

    #region Private Methods
    /// <summary>
    /// Handles the event when a new order is started.
    /// </summary>
    /// <param name="order">The order that was started.</param>
    private void OnOrderStarted(OrderViewModel order)
    {
    }

    /// <summary>
    /// Handles the cancellation of the current payment process.
    /// Shows a confirmation dialog before canceling.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Handles navigation to this page.
    /// Initializes checkout service and loads existing order and product data.
    /// </summary>
    /// <param name="parameter">Navigation parameter.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation from this page.
    /// Performs cleanup of resources and stops the checkout service.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnNavigatedFrom()
    {
        OrderCartViewModel.Dispose();
        _checkoutService.StopAsync();
        _messenger.UnregisterAll(this);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles checkout request messages.
    /// Notifies the checkout service when a checkout is requested for a table.
    /// </summary>
    /// <param name="message">Message containing the table ID for checkout.</param>
    public void Receive(CheckoutRequestedMessage message)
    {
        _checkoutService.NotifyCheckoutRequestedAsync(message.TableId ?? 0);
    }
    #endregion
}