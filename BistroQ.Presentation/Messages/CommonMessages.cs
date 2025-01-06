using System.Collections.ObjectModel;

namespace BistroQ.Presentation.Messages;

/// <summary>
/// Record representing a message to change the selection state of a CustomListView.
/// Used to programmatically control selection in a specific CustomListView instance.
/// </summary>
/// <typeparam name="T">The type of items in the CustomListView.</typeparam>
/// <param name="SelectedItems">Collection of items to be set as selected.</param>
/// <param name="Title">Identifier of the target CustomListView, matching its Tag property.</param>
public record ChangeCustomListViewSelectionMessage<T>(ObservableCollection<T> SelectedItems, string Title);

/// <summary>
/// Record representing a message sent when the selection in a CustomListView has changed.
/// Notifies subscribers about selection changes in a specific CustomListView instance.
/// </summary>
/// <typeparam name="T">The type of items in the CustomListView.</typeparam>
/// <param name="SelectedItems">Collection of currently selected items after the change.</param>
/// <param name="Title">Identifier of the CustomListView where selection changed, matching its Tag property.</param>
public record CustomListViewSelectionChangedMessage<T>(ObservableCollection<T> SelectedItems, string Title);