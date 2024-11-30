using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.Controls;

public partial class CustomListView : ListView, IRecipient<CustomListViewSelectionChangedMessage>
{
    public CustomListView() : base()
    {
        SelectionChanged += CustomListView_SelectionChanged;
        App.GetService<IMessenger>().RegisterAll(this);
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
    }

    public void Receive(CustomListViewSelectionChangedMessage message)
    {
        if (message.Title == (string)Tag)
        {
            Debug.WriteLine("Received " + message.Title);

            SelectedItems = message.SelectedItems;

            Debug.WriteLine(base.SelectedItems.Count);
        }
    }
}