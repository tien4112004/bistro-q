using BistroQ.Presentation.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace BistroQ.Presentation.Controls;
/// <summary>
/// A custom ListView implementation that supports multiple selection with observable collection binding
/// and messaging capabilities. Inherits from ListView and implements messaging and disposal interfaces.
/// </summary>
/// <typeparam name="T">The type of items contained in the ListView.</typeparam>
/// <remarks>
/// Enhances the base ListView by:
/// - Supporting two-way binding for selected items
/// - Implementing messaging for selection changes
/// - Managing proper cleanup of resources
/// </remarks>
public partial class CustomListView<T> : ListView, IRecipient<ChangeCustomListViewSelectionMessage<T>>, IDisposable
{
    #region Private Fields
    /// <summary>
    /// The messenger service for handling selection change communications.
    /// </summary>
    private readonly IMessenger _messenger;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the CustomListView class.
    /// Sets up event handlers, registers for messages, and configures cleanup.
    /// </summary>
    public CustomListView() : base()
    {
        SelectionChanged += CustomListView_SelectionChanged;
        _messenger = App.GetService<IMessenger>();
        _messenger.RegisterAll(this);
        this.Unloaded += (sender, e) => Dispose();
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets the collection of selected items.
    /// Provides two-way binding support for selected items in the ListView.
    /// </summary>
    /// <remarks>
    /// This property overrides the base SelectedItems to provide better binding support
    /// with an ObservableCollection.
    /// </remarks>
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

    /// <summary>
    /// Dependency property for the SelectedItems property.
    /// Enables WPF binding and property change notification.
    /// </summary>
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.Register(
            nameof(SelectedItems),
            typeof(ObservableCollection<T>),
            typeof(CustomListView<T>),
            new PropertyMetadata(new ObservableCollection<T>()));
    #endregion

    #region Private Methods
    /// <summary>
    /// Handles the SelectionChanged event of the ListView.
    /// Updates the SelectedItems collection and sends a notification message.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing the changed items.</param>
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
    #endregion

    #region Public Methods
    /// <summary>
    /// Handles messages to change the ListView's selection.
    /// Only processes messages meant for this specific ListView (matched by Tag).
    /// </summary>
    /// <param name="message">The message containing selection change information.</param>
    public void Receive(ChangeCustomListViewSelectionMessage<T> message)
    {
        if (message.Title == (string)Tag)
        {
            SelectedItems = new ObservableCollection<T>(message.SelectedItems);
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
    /// Unregisters from the messenger service.
    /// </summary>
    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }
    #endregion
}

