using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient, INavigationAware
{
    private readonly IOrderDataService _orderDataService;

    public Order Order { get; set; }

    [ObservableProperty]
    private bool _isOrdering;

    [ObservableProperty]
    private bool _isLoading = false;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ICommand StartOrderCommand { get; }
    public ICommand CancelOrderCommand { get; }
    public HomePageViewModel(IOrderDataService orderDataService)
    {
        _orderDataService = orderDataService;
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
        CancelOrderCommand = new RelayCommand(CancelOrder);
    }

    private async Task StartOrder()
    {
        try
        {
            IsLoading = true;

            await Task.Delay(400); // TODO: This if for UI visualize only, remove it afterward  

            Order = await Task.Run(
                async () =>
                {
                    return await _orderDataService.CreateOrderAsync();
                });
            IsOrdering = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void CancelOrder()
    {
        _orderDataService.DeleteOrderAsync();
        Order = null;

        ErrorMessage = string.Empty;
        IsOrdering = false;
    }

    public async void OnNavigatedTo(object parameter)
    {
        IsLoading = true;
        var existingOrder = await Task.Run(async () =>
        {
            return await _orderDataService.GetOrderAsync();

        });

        IsLoading = false;

        if (existingOrder == null)
        {
            return;
        }
        Order = existingOrder;
        IsOrdering = true;
    }

    public void OnNavigatedFrom()
    {
        //
    }
}
