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
    private bool _canComplete;

    [ObservableProperty]
    private bool _canMove;

    [ObservableProperty]
    private bool _canCancel;

    public KitchenOrderButtonsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }
    public void UpdateStates(IEnumerable<KitchenOrderItemViewModel> items)
    {
        Items = new ObservableCollection<KitchenOrderItemViewModel>(items);
        CanComplete = Items.Any(item => item.Status == OrderItemStatus.InProgress);
        CanMove = Items.Any();
        CanCancel = Items.Any(item => item.Status == OrderItemStatus.Pending);
    }

    [RelayCommand]
    private void Complete()
    {
        if (CanComplete)
            _messenger.Send(new KitchenActionMessage(Items, KitchenAction.Complete));
    }

    [RelayCommand]
    private void Move()
    {
        if (CanMove)
            _messenger.Send(new KitchenActionMessage(Items, KitchenAction.Move));
    }

    [RelayCommand]
    private void Cancel()
    {
        if (CanCancel)
            _messenger.Send(new KitchenActionMessage(Items, KitchenAction.Cancel));
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
}
