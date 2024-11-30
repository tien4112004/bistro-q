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

    public ObservableCollection<KitchenOrderItemViewModel> Items { get; set; } = new();

    [ObservableProperty]
    private bool _isCompleteButtonEnable;

    [ObservableProperty]
    private bool _isMoveButtonEnable;

    [ObservableProperty]
    private bool _isCancelButtonEnable;

    public KitchenOrderButtonsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }
    public void UpdateStates(IEnumerable<KitchenOrderItemViewModel> items)
    {
        Items = new ObservableCollection<KitchenOrderItemViewModel>(items);
        IsCompleteButtonEnable = Items.Any(item => item.Status == OrderItemStatus.InProgress);
        IsMoveButtonEnable = Items.Any();
        IsCancelButtonEnable = Items.Any(item => item.Status == OrderItemStatus.Pending);
    }

    [RelayCommand]
    private void Complete()
    {
        if (IsCompleteButtonEnable)
            _messenger.Send(new KitchenActionMessage(Items.Select(item => item.OrderItemId), KitchenAction.Complete));
    }

    [RelayCommand]
    private void Move()
    {
        if (IsMoveButtonEnable)
            _messenger.Send(new KitchenActionMessage(Items.Select(item => item.OrderItemId), KitchenAction.Move));
    }

    [RelayCommand]
    private void Cancel()
    {
        if (IsCancelButtonEnable)
            _messenger.Send(new KitchenActionMessage(Items.Select(item => item.OrderItemId), KitchenAction.Cancel));
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}
