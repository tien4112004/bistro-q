namespace BistroQ.Presentation.Messages;

/// <summary>
/// Record representing a message sent when the page size for pagination changes.
/// Used to notify components about changes in the number of items displayed per page.
/// </summary>
/// <param name="NewPageSize">The new number of items to display per page.</param>
public record PageSizeChangedMessage(int NewPageSize);

/// <summary>
/// Record representing a message sent when the current page number changes.
/// Used to notify components about navigation to a different page in paginated data.
/// </summary>
/// <param name="NewCurrentPage">The new current page number (1-based indexing).</param>
public record CurrentPageChangedMessage(int NewCurrentPage);

/// <summary>
/// Record representing a message sent when pagination state changes.
/// Provides comprehensive information about the current pagination state.
/// </summary>
/// <param name="TotalItems">The total number of items across all pages.</param>
/// <param name="CurrentPage">The current page number (1-based indexing).</param>
/// <param name="TotalPages">The total number of available pages.</param>
/// <remarks>
/// This message typically occurs when:
/// - Data is initially loaded
/// - Data is filtered or searched
/// - Total number of items changes
/// </remarks>
public record PaginationChangedMessage(int TotalItems, int CurrentPage, int TotalPages);