using BistroQ.Domain.Enums;
using BistroQ.Presentation.Enums;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;
public partial class KitchenOrderButtonsViewModel : ObservableObject, IDisposable
{
    private readonly IMessenger _messenger;

    public ObservableCollection<OrderItemViewModel> Items { get; set; } = new();

    [ObservableProperty]
    private bool _canComplete;

    [ObservableProperty]
    private bool _canMove;

    [ObservableProperty]
    private bool _canCancel;

    [ObservableProperty]
    private bool _isCompleteLoading;

    [ObservableProperty]
    private bool _isMoveLoading;

    [ObservableProperty]
    private bool _isCancelLoading;

    public KitchenOrderButtonsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }
    public void UpdateStates(IEnumerable<OrderItemViewModel> items)
    {
        Items = new ObservableCollection<OrderItemViewModel>(items);
        CanComplete = Items.Any(item => item.Status == OrderItemStatus.InProgress);
        CanMove = Items.Any();
        CanCancel = Items.Any(item => item.Status == OrderItemStatus.Pending);
    }

    [RelayCommand]
    private void Complete()
    {
        if (CanComplete)
        {
            CanComplete = false;
            CanMove = false;
            CanCancel = false;
            IsCompleteLoading = true;
            _messenger.Send(new KitchenActionMessage(KitchenAction.Complete));
        }
    }

    [RelayCommand]
    private void Move()
    {
        if (CanMove)
        {
            CanComplete = false;
            CanMove = false;
            CanCancel = false;
            IsMoveLoading = true;
            _messenger.Send(new KitchenActionMessage(KitchenAction.Move));
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        if (CanCancel)
        {
            CanComplete = false;
            CanMove = false;
            CanCancel = false;
            IsCancelLoading = true;
            _messenger.Send(new KitchenActionMessage(KitchenAction.Cancel));
        }
    }

    public void ResetLoadingState()
    {
        IsCompleteLoading = false;
        IsMoveLoading = false;
        IsCancelLoading = false;
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}
