using System.Collections;

namespace BistroQ.Presentation.Messages;

public record CustomListViewSelectionChangedMessage(IList SelectedItems, string Title);