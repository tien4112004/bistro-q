using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenHistory;

public partial class OrderItemGridViewModel :
    ObservableObject,
    IDisposable,
    IRecipient<OrderGridStatusChangedMessage>,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    private readonly IMessenger _messenger;
    private readonly IMapper _mapper;
    private readonly IOrderItemDataService _dataService;
    private readonly IDialogService _dialogService;
    private bool _isDisposed;

    [ObservableProperty]
    private OrderItemGridState _state = new();

    public OrderItemGridViewModel(
        IMessenger messenger,
        IMapper mapper,
        IDialogService dialogService,
        IOrderItemDataService dataService)
    {
        _messenger = messenger;
        _mapper = mapper;
        _dialogService = dialogService;
        _dataService = dataService;
        _messenger.RegisterAll(this);
    }

    public async Task LoadItemsAsync()
    {
        if (State.IsLoading || _isDisposed) return;
        try
        {
            State.IsLoading = true;

            var result = await TaskHelper.WithMinimumDelay(_dataService.GetOrderItemsAsync(State.Query), 200);

            var items = _mapper.Map<IEnumerable<OrderItemViewModel>>(result.Data);
            State.Items = new ObservableCollection<OrderItemViewModel>(items);
            _messenger.Send(new PaginationChangedMessage(
                result.TotalItems,
                result.CurrentPage,
                result.TotalPages
             ));
        }
        catch (Exception ex)
        {
            if (ex is ObjectDisposedException e)
            {
                Debug.WriteLine(
                        "Disposed object details:\n" +
                        $"Object Name: {e.ObjectName}\n" +
                        $"Message: {e.Message}\n" +
                        $"Source: {e.Source}\n" +
                        $"Stack Trace: {e.StackTrace}"
                    );
            }
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            State.IsLoading = false;
        }
    }


    public void Receive(CurrentPageChangedMessage message)
    {
        State.Query.Page = message.NewCurrentPage;
        _ = LoadItemsAsync();
    }

    public void Receive(OrderGridStatusChangedMessage message)
    {
        State.Reset();
        State.Query.Status = message.Status.ToString();
        State.ReturnToFirstPage();
        _ = LoadItemsAsync();
    }

    public void Receive(PageSizeChangedMessage message)
    {
        State.Query.Size = message.NewPageSize;
        State.ReturnToFirstPage();
        _ = LoadItemsAsync();
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        _messenger.UnregisterAll(this);
    }
}
