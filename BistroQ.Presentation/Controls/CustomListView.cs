using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.Controls;

public partial class CustomListView<T> : ListView, IRecipient<ChangeCustomListViewSelectionMessage<T>>, IDisposable
{
    private readonly IMessenger _messenger;
    private bool _disposed;

    public CustomListView() : base()
    {
        SelectionChanged += CustomListView_SelectionChanged;
        _messenger = App.GetService<IMessenger>();
        _messenger.RegisterAll(this);
    }

    public new ObservableCollection<T> SelectedItems
    {
        get => (ObservableCollection<T>)GetValue(SelectedItemsProperty);
        set
        {
            SetValue(SelectedItemsProperty, value);
            base.SelectedItems.Clear();
            foreach (var item in value)
            {
                base.SelectedItems.Add(item);
            }
        }
    }

    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(ObservableCollection<T>),
            typeof(CustomListView<T>),
            new PropertyMetadata(new ObservableCollection<T>()));

    private void CustomListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (var item in e.RemovedItems.OfType<T>())
        {
            SelectedItems.Remove(item);
        }

        foreach (var item in e.AddedItems.OfType<T>())
        {
            SelectedItems.Add(item);
        }

        _messenger.Send(new CustomListViewSelectionChangedMessage<T>(SelectedItems, (string)Tag));
    }

    public void Receive(ChangeCustomListViewSelectionMessage<T> message)
    {
        if (message.Title == (string)Tag)
        {
            SelectedItems = new ObservableCollection<T>(message.SelectedItems);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _messenger.UnregisterAll(this);
    }
}