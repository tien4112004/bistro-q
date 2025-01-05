using CommunityToolkit.Mvvm.ComponentModel;

namespace BistroQ.Presentation.ViewModels.Commons;

/// <summary>
/// ViewModel responsible for managing pagination state and calculations.
/// </summary>
/// <remarks>
/// Implements ObservableObject to provide change notifications for pagination properties.
/// Includes validation to ensure valid page numbers and maintain pagination state.
/// </remarks>
public partial class PaginationViewModel : ObservableObject
{
    /// <summary>
    /// Backing field for CurrentPage property
    /// </summary>
    private int _currentPage;

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    /// <remarks>
    /// Validates the page number to ensure it's a positive integer.
    /// If an invalid value is provided (NaN or less than 1), defaults to page 1.
    /// </remarks>
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (double.IsNaN(value) || value <= 0)
            {
                value = 1;
            }
            else
            {
                _currentPage = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the total number of pages available.
    /// </summary>
    [ObservableProperty]
    private int _totalPages = 0;

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    [ObservableProperty]
    private int _totalItems = 0;

    /// <summary>
    /// Gets or sets the number of items displayed per page.
    /// Defaults to 10 items per page.
    /// </summary>
    [ObservableProperty]
    private int _pageSize = 10;
}