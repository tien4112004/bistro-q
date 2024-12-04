namespace BistroQ.Presentation.Messages;

public record PageSizeChangedMessage(int NewPageSize);
public record CurrentPageChangedMessage(int NewCurrentPage);
public record PaginationChangedMessage(int TotalItems, int CurrentPage, int TotalPages);