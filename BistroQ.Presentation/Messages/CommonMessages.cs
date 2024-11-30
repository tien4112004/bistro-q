using System.Collections;

namespace BistroQ.Presentation.Messages;

public record ChangeCustomListViewSelectionMessage(IList SelectedItems, string Title);

public record CustomListViewSelectionChangedMessage(IList SelectedItems, string Title);