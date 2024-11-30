using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.Controls;

public partial class CustomListView : ListView, IRecipient<ChangeCustomListViewSelectionMessage>
{
    private readonly IMessenger _messenger;
    public CustomListView() : base()
    {
        SelectionChanged += CustomListView_SelectionChanged;
        _messenger = App.GetService<IMessenger>();
        _messenger.RegisterAll(this);
    }

    public new IList SelectedItems
    {
        get
        {
            return (IList)GetValue(SelectedItemsProperty);
        }
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

    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        nameof(SelectedItems),
        typeof(IList),
        typeof(CustomListView),
        new PropertyMetadata(new ObservableCollection<object>()));

    private void CustomListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        foreach (var item in e.RemovedItems)
        {
            SelectedItems.Remove(item);
        }

        foreach (var item in e.AddedItems)
        {
            SelectedItems.Add(item);
        }

        _messenger.Send(new CustomListViewSelectionChangedMessage(SelectedItems, (string)Tag));
    }

    public void Receive(ChangeCustomListViewSelectionMessage message)
    {
        if (message.Title == (string)Tag)
        {
            SelectedItems = message.SelectedItems;
        }
    }
}