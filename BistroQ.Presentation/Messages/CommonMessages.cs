using System.Collections.ObjectModel;

namespace BistroQ.Presentation.Messages;

public record ChangeCustomListViewSelectionMessage<T>(ObservableCollection<T> SelectedItems, string Title);

public record CustomListViewSelectionChangedMessage<T>(ObservableCollection<T> SelectedItems, string Title);